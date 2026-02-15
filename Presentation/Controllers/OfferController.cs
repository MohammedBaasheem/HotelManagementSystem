using Application.DTOs.OfferDTOs;
using Application.Services;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModels.OfferVMs;
using Presentation.ViewModels.ResponseVMs;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController : ControllerBase
    {
        private readonly OfferManagmentService _offerManagmentService;
        private readonly IMapper _mapper;

        public OfferController(OfferManagmentService offerManagmentService, IMapper mapper)
        {
            _offerManagmentService=offerManagmentService;
            _mapper=mapper;
        }
        private Guid CurrentUserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        [HttpGet("{id}")]
        public async Task<ResponseVM<GetOfferVM>> GetById(Guid id)
        {
            var resultDto = await _offerManagmentService.GetOfferByIdAsync(id);

            return _mapper.Map<ResponseVM<GetOfferVM>>(resultDto);
        }

        [HttpGet("all")]
        public async Task<ResponseVM<IEnumerable<GetOfferVM>>> GetAll()
        {
            var resultDto = await _offerManagmentService.GetAllOffersAsync();

            return _mapper.Map<ResponseVM<IEnumerable<GetOfferVM>>>(resultDto);
        }

        [HttpGet("filter")]
        public async Task<ResponseVM<IEnumerable<GetOfferVM>>> GetFiltered([FromQuery] OffersFilterDto filter)
        {
            var resultDto = await _offerManagmentService.GetFilteredOffersAsync(filter);

            return _mapper.Map<ResponseVM<IEnumerable<GetOfferVM>>>(resultDto);
        }

        [HttpPost]
        public async Task<ResponseVM<bool>> Create([FromBody] CreateOfferVM requestVM)
        {
            var userId = CurrentUserId;

            var dto = _mapper.Map<CreateOfferDto>(requestVM);
            var resultDto = await _offerManagmentService.CreateOfferAsync(dto, userId);
            return _mapper.Map<ResponseVM<bool>>(resultDto);
        }

        [HttpPut]
        public async Task<ResponseVM<bool>> Update([FromBody] UpdateOfferVM requestVM)
        {
            var userId = CurrentUserId;

            var dto = _mapper.Map<UpdateOfferDto>(requestVM);

            var resultDto = await _offerManagmentService.UpdateOfferAsync(dto, userId);

            return _mapper.Map<ResponseVM<bool>>(resultDto);
        }

        [HttpDelete("{id}")]
        public async Task<ResponseVM<bool>> Delete(Guid id)
        {
            var resultDto = await _offerManagmentService.DeleteOfferAsync(id);

            return _mapper.Map<ResponseVM<bool>>(resultDto);
        }
    }
}
