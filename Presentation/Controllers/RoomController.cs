using Application.DTOs.FacilityDTOs;
using Application.DTOs.RoomDTOs;
using Application.DTOs.RoomFacilityDTOs;
using Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomManagmentService _roomManagmentService;

        public RoomController(IRoomManagmentService roomManagmentService)
        {
            _roomManagmentService = roomManagmentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom(CreateRoomDto dto)
        {
            var result = await _roomManagmentService.CreateRoomAsync(dto);
            return Ok(result);
        }
        [HttpPost("facility")]
        public async Task<IActionResult> CreateFacility(CreateFacilityDto dto)
        {
            var result = await _roomManagmentService.CreateFacilityAsync(dto);
            return Ok(result);
        }
        [HttpPost("assign-facility")]
        public async Task<IActionResult> AssignFacilityToRoom(AssignFacilityToRoomDto dto)
        {
            var result = await _roomManagmentService.AssignFacilityToRoomAsync(dto);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRooms()
        {
            var result = await _roomManagmentService.GetAllRoomsAsync();
            return Ok(result);
        }
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetRoomById(Guid id)
        {
            var result = await _roomManagmentService.GetRoomByIdAsync(id);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateRoom(UpdateRoomDto dto)
        {
            var result = await _roomManagmentService.UpdateRoomAsync(dto);
            return Ok(result);
        }
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteRoom(Guid id)
        {
            var result = await _roomManagmentService.DeleteRoomAsync(id);
            return Ok(result);

        }
        [HttpGet("facility")]
        public async Task<IActionResult> GetAllFacilities()
        {
            var result = await _roomManagmentService.GetAllFacilitiesAsync();
            return Ok(result);
        }
        [HttpGet("facility/{id:guid}")]
        public async Task<IActionResult> GetFacilityById(Guid id)
        {
            var result = await _roomManagmentService.GetFacilityByIdAsync(id);
            return Ok(result);
        }
        [HttpPut("facility")]
        public async Task<IActionResult> UpdateFacility(UpdateFacilityDto dto)
        {
            var result = await _roomManagmentService.UpdateFacilityAsync(dto);
            return Ok(result);
        }
        [HttpDelete("facility/{id:guid}")]
        public async Task<IActionResult> DeleteFacility(Guid id)
        {
            var result = await _roomManagmentService.DeleteFacilityAsync(id);
            return Ok(result);
        }
        [HttpGet("{roomId:guid}/facilities")]
        public async Task<IActionResult> GetFacilitiesByRoomId(Guid roomId)
        {
            var result = await _roomManagmentService.GetFacilitiesByRoomIdAsync(roomId);
            return Ok(result);
        }
        [HttpDelete("{roomId:guid}/facilities/{facilityId:guid}")]
        public async Task<IActionResult> UnassignFacility(Guid roomId, Guid facilityId)
        {
            var result = await _roomManagmentService.UnassignFacilityFromRoomAsync(roomId, facilityId);
            return Ok(result);
        }

    }
}
