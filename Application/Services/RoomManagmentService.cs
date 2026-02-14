using Application.DTOs.FacilityDTOs;
using Application.DTOs.ResponseDTOs;
using Application.DTOs.RoomDTOs;
using Application.DTOs.RoomFacilityDTOs;
using Application.Interface;
using Application.IRepositories;
using Domain.Enums;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class RoomManagmentService : IRoomManagmentService
    {
        private readonly IGeneralRepository<Room> _roomRepository;
        private readonly IGeneralRepository<Facility> _facilityRepository;
        private readonly IGeneralRepository<RoomFacility> _roomFacilityRepository;

        


        public RoomManagmentService(
            IGeneralRepository<Room> roomRepository
            , IGeneralRepository<Facility> facilityRepository,
           IGeneralRepository<RoomFacility> roomFacilityRepository)
        {
            _roomRepository = roomRepository;
            _facilityRepository = facilityRepository;
            _roomFacilityRepository = roomFacilityRepository;
        }

        public async Task<ResponseDto<Guid>> CreateRoomAsync(CreateRoomDto dto)
        {
            var room = new Room
            {
                Code = dto.Code,
                PricePerNight = dto.PricePerNight,
                FloorNumber = dto.FloorNumber,
                RoomDescription = dto.RoomDescription,
                RoomType = dto.RoomType,
                UserId = dto.UserId
            };

            await _roomRepository.AddAsync(room);

            return ResponseDto<Guid>.Success(room.Id, "Room created successfully");
        }

        public async Task<ResponseDto<Guid>> CreateFacilityAsync(CreateFacilityDto dto)
        {
            var facility = new Facility
            {
                FacilityName = dto.FacilityName,
                Description = dto.Description,
                Price = dto.Price
            };

            await _facilityRepository.AddAsync(facility);

            return ResponseDto<Guid>.Success(facility.Id, "Facility created successfully");
        }
        public async Task<ResponseDto<bool>> AssignFacilityToRoomAsync(AssignFacilityToRoomDto dto)
        {
            var roomFacility = new RoomFacility
            {
                RoomId = dto.RoomId,
                FacilityId = dto.FacilityId
            };

            await _roomFacilityRepository.AddAsync(roomFacility);

            return ResponseDto<bool>.Success(true, "Facility assigned to room successfully");
        }
        public async Task<ResponseDto<List<RoomDto>>> GetAllRoomsAsync()
        {
            var rooms = _roomRepository.GetAll();

            var result = rooms
                .Where(r => r.IsActive)
                .Select(r => new RoomDto
                {
                    Id = r.Id,
                    Code = r.Code,
                    FloorNumber = r.FloorNumber,
                    PricePerNight = r.PricePerNight,
                    RoomDescription = r.RoomDescription,
                    RoomType = r.RoomType,
                    UserId = r.UserId,
                    IsActive = r.IsActive
                })
                .ToList();

            return ResponseDto<List<RoomDto>>.Success(result);
        }
        public async Task<ResponseDto<RoomDto>> GetRoomByIdAsync(Guid id)
        {
            var room = await _roomRepository.GetById(id);

            if (room == null || !room.IsActive)
                return ResponseDto<RoomDto>.Fail(ErrorCode.ValidationError, "Room not found");

            var dto = new RoomDto
            {
                Id = room.Id,
                Code = room.Code,
                FloorNumber = room.FloorNumber,
                PricePerNight = room.PricePerNight,
                RoomDescription = room.RoomDescription,
                RoomType = room.RoomType,
                UserId = room.UserId,
                IsActive = room.IsActive
            };

            return ResponseDto<RoomDto>.Success(dto);
        }

        public async Task<ResponseDto<bool>> UpdateRoomAsync(UpdateRoomDto dto)
        {
            var room = await _roomRepository.GetWithTrackingById(dto.Id);

            if (room == null)
                return ResponseDto<bool>.Fail(ErrorCode.ValidationError, "Room not found");

            room.Code = dto.Code;
            room.PricePerNight = dto.PricePerNight;
            room.FloorNumber = dto.FloorNumber;
            room.RoomDescription = dto.RoomDescription;
            room.RoomType = dto.RoomType;
            room.UserId = dto.UserId;
            room.IsActive = dto.IsActive;

            _roomRepository.Update(room);
            await _roomRepository.SaveChanges();

            return ResponseDto<bool>.Success(true, "Room updated successfully");
        }
        public async Task<ResponseDto<bool>> DeleteRoomAsync(Guid id)
        {
            var room = await _roomRepository.GetWithTrackingById(id);

            if (room == null)
                return ResponseDto<bool>.Fail(ErrorCode.ValidationError, "Room not found");

            _roomRepository.SoftDelete(room);
            await _roomRepository.SaveChanges();

            return ResponseDto<bool>.Success(true, "Room deleted successfully");
        }
        public async Task<ResponseDto<List<FacilityDto>>> GetAllFacilitiesAsync()
        {
            var facilities = _facilityRepository.GetAll();

            var result = facilities
                .Where(f => f.IsActive)
                .Select(f => new FacilityDto
                {
                    Id = f.Id,
                    FacilityName = f.FacilityName,
                    Description = f.Description,
                    Price = f.Price,
                    IsActive = f.IsActive
                })
                .ToList();

            return ResponseDto<List<FacilityDto>>.Success(result);
        }
        public async Task<ResponseDto<FacilityDto>> GetFacilityByIdAsync(Guid id)
        {
            var facility = await _facilityRepository.GetById(id);

            if (facility == null || !facility.IsActive)
                return ResponseDto<FacilityDto>.Fail(ErrorCode.ValidationError, "Facility not found");

            var dto = new FacilityDto
            {
                Id = facility.Id,
                FacilityName = facility.FacilityName,
                Description = facility.Description,
                Price = facility.Price,
                IsActive = facility.IsActive
            };

            return ResponseDto<FacilityDto>.Success(dto);
        }
        public async Task<ResponseDto<bool>> UpdateFacilityAsync(UpdateFacilityDto dto)
        {
            var facility = await _facilityRepository.GetWithTrackingById(dto.Id);

            if (facility == null)
                return ResponseDto<bool>.Fail(ErrorCode.ValidationError, "Facility not found");

            facility.FacilityName = dto.FacilityName;
            facility.Description = dto.Description;
            facility.Price = dto.Price;
            facility.IsActive = dto.IsActive;

            _facilityRepository.Update(facility);
            await _facilityRepository.SaveChanges();

            return ResponseDto<bool>.Success(true, "Facility updated successfully");
        }
        public async Task<ResponseDto<bool>> DeleteFacilityAsync(Guid id)
        {
            var facility = await _facilityRepository.GetWithTrackingById(id);

            if (facility == null)
                return ResponseDto<bool>.Fail(ErrorCode.ValidationError, "Facility not found");

            _facilityRepository.SoftDelete(facility);
            await _facilityRepository.SaveChanges();

            return ResponseDto<bool>.Success(true, "Facility deleted successfully");
        }
        public async Task<ResponseDto<List<RoomFacilityDto>>> GetFacilitiesByRoomIdAsync(Guid roomId)
        {
            var roomFacilities = _roomFacilityRepository.GetAll();

            var result = roomFacilities
                .Where(rf => rf.RoomId == roomId && rf.IsActive)
                .Select(rf => new RoomFacilityDto
                {
                    Id = rf.Id,
                    RoomId = rf.RoomId,
                    FacilityId = rf.FacilityId,
                    IsActive = rf.IsActive
                })
                .ToList();

            return ResponseDto<List<RoomFacilityDto>>.Success(result);
        }
        public async Task<ResponseDto<bool>> UnassignFacilityFromRoomAsync(Guid roomId, Guid facilityId)
        {
            var link = await _roomFacilityRepository.GetWithTracking(rf =>
                rf.RoomId == roomId && rf.FacilityId == facilityId && rf.IsActive);

            if (link == null)
                return ResponseDto<bool>.Fail(ErrorCode.ValidationError, "RoomFacility link not found");

            _roomFacilityRepository.SoftDelete(link);
            await _roomFacilityRepository.SaveChanges();

            return ResponseDto<bool>.Success(true, "Facility unassigned successfully");
        }



    }
}