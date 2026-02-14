using Application.DTOs.OfferDTOs;
using Application.DTOs.ResponseDTOs;
using Application.Helpers;
using Application.Interface;
using Application.IRepositories;
using AutoMapper;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class OfferManagmentService
    {
        protected readonly IOfferManagmentRepository _offerRepo;
        protected readonly IRoomManagmentRepository _roomRepo;
        protected readonly IValidationService _validationService;
        protected readonly IMapper _mapper;

        public OfferManagmentService(
            IOfferManagmentRepository offerRepo,
            IRoomManagmentRepository roomRepo,
            IValidationService validationService,
            IMapper mapper)
        {
            _offerRepo = offerRepo;
            _roomRepo = roomRepo;
            _validationService = validationService;
            _mapper = mapper;
        }

        public async Task<ResponseDto<bool>> CreateOfferAsync(CreateOfferDto requestDto,Guid CurrentUserId)
        {
            var errors = await _validationService.ValidateAsync(requestDto);
            if (errors.Any())
            {
                return ResponseDto<bool>.Fail(ErrorCode.ValidationError, "Validation Error", errors);
            }

            var offer = _mapper.Map<Offer>(requestDto);

            string systemCode;
            do
            {
                systemCode = Helpers.CodeOfferGenerator.GenerateRandomCode();
            }
            while (await _offerRepo.IsExistAsync(o => o.Code == systemCode)); 

            offer.Code = systemCode;

            if (requestDto.RoomIds != null && requestDto.RoomIds.Any())
            {
                var distinctRoomIds = requestDto.RoomIds.Distinct().ToList();

                var existingIds = await _roomRepo.GetFilter(
                    predicate: r => distinctRoomIds.Contains(r.Id) && !r.IsDeleted,
                    selector: r => r.Id
                );

                if (existingIds.Count() != distinctRoomIds.Count)
                {
                    var missingIds = distinctRoomIds.Except(existingIds);
                    var missingStr = string.Join(", ", missingIds);

                    return ResponseDto<bool>.Fail(ErrorCode.RoomNotFound, $"The following Room IDs do not exist: {missingStr}");
                }

                offer.RoomOffer = distinctRoomIds.Select(id => new RoomOffer
                {
                    
                    RoomId = id
                }).ToHashSet(); 
            }

            else
            {
                offer.RoomOffer = new HashSet<RoomOffer>();
            }
            offer.IsActive = true; 
            offer.CreatedAt = DateTime.UtcNow; 
            offer.CreatedBy = CurrentUserId;
            await _offerRepo.AddAsync(offer);
            await _offerRepo.SaveChanges();

            return ResponseDto<bool>.Success(true, $"Offer created successfully. Code: {systemCode}");
        }

        public async Task<ResponseDto<bool>> UpdateOfferAsync(UpdateOfferDto requestDto, Guid CurrentUserId)
        {
            var errors = await _validationService.ValidateAsync(requestDto);
            if (errors.Any())
            {
                return ResponseDto<bool>.Fail(ErrorCode.ValidationError, "Validation Error", errors);
            }

            var offer = await _offerRepo.GetWithTrackingById(requestDto.Id);

            if (offer == null)
            {
                return ResponseDto<bool>.Fail(ErrorCode.OfferNotFound, "Offer not found");
            }

            _mapper.Map(requestDto, offer);

            List<Guid> distinctRoomIds = new List<Guid>();

            if (requestDto.RoomIds != null && requestDto.RoomIds.Any())
            {
                distinctRoomIds = requestDto.RoomIds.Distinct().ToList();

                var existingIds = await _roomRepo.GetFilter(
                    predicate: r => distinctRoomIds.Contains(r.Id) && !r.IsDeleted,
                    selector: r => r.Id
                );

                if (existingIds.Count() != distinctRoomIds.Count)
                {
                    var missingIds = distinctRoomIds.Except(existingIds);
                    var missingStr = string.Join(", ", missingIds);
                    return ResponseDto<bool>.Fail(ErrorCode.RoomNotFound, $"The following Room IDs do not exist: {missingStr}");
                }
            }

            offer.UpdatedAt = DateTime.UtcNow;
            offer.UpdatedBy = CurrentUserId;

            await _offerRepo.UpdateOfferRoomsAsync(offer.Id, distinctRoomIds);
            _offerRepo.Update(offer); 
            await _offerRepo.SaveChanges();

            return ResponseDto<bool>.Success(true, "Offer updated successfully");
        }

        public async Task<ResponseDto<bool>> DeleteOfferAsync(Guid id)
        {
            var offer = await _offerRepo.GetById(id);

            if (offer == null || offer.IsDeleted)
            {
                return ResponseDto<bool>.Fail(ErrorCode.OfferNotFound, "Offer not found or already deleted");
            }

            _offerRepo.SoftDelete(offer);

            await _offerRepo.SaveChanges();

            return ResponseDto<bool>.Success(true, "Offer deleted successfully");
        }

        public async Task<ResponseDto<IEnumerable<GetOfferDto>>> GetFilteredOffersAsync(OffersFilterDto filter)
        {
            var query = _offerRepo.GetAll();

            
            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                query = query.Where(x => x.Code.Contains(filter.SearchText) || x.Description.Contains(filter.SearchText));
            }

            
            if (filter.SearchDate.HasValue)
            {
                query = query.Where(x => x.StartDate <= filter.SearchDate.Value && x.EndDate >= filter.SearchDate.Value);
            }

            
            if (filter.IsActiveNow.HasValue)
            {
                var now = DateTime.UtcNow;

                if (filter.IsActiveNow.Value)
                {
                   
                    query = query.Where(x => x.IsActive && x.StartDate <= now && x.EndDate >= now);
                }
                else
                {
                    query = query.Where(x => !x.IsActive || x.StartDate > now || x.EndDate < now);
                }
            }

            if (filter.MinDiscount.HasValue)
                query = query.Where(x => x.DiscountValue >= filter.MinDiscount.Value);

            if (filter.MaxDiscount.HasValue)
                query = query.Where(x => x.DiscountValue <= filter.MaxDiscount.Value);


            query = query.OrderByDescending(x => x.DiscountValue)
                         .ThenByDescending(x => x.CreatedAt);

            var result = await query.Map<GetOfferDto>().ToListAsync();

            return ResponseDto<IEnumerable<GetOfferDto>>.Success(result);
        }

        public async Task<ResponseDto<GetOfferDto>> GetOfferByIdAsync(Guid id)
        {
            var offerDto = await _offerRepo.GetAll()
                .Where(x => x.Id == id)
                .Map<GetOfferDto>() 
                .FirstOrDefaultAsync(); 

            if (offerDto == null)
            {
                return ResponseDto<GetOfferDto>.Fail(ErrorCode.OfferNotFound, "Offer not found");
            }

            return ResponseDto<GetOfferDto>.Success(offerDto);
        }


        public async Task<ResponseDto<IEnumerable<GetOfferDto>>> GetAllOffersAsync()
        {
            var offersList = await _offerRepo.GetAll()
                .Map<GetOfferDto>()
                .ToListAsync();

            return ResponseDto<IEnumerable<GetOfferDto>>.Success(offersList);
        }

    }
}
