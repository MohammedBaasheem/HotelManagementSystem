using Application.DTOs.FacilityDTOs;
using Application.DTOs.ResponseDTOs;
using Application.DTOs.RoomDTOs;
using Application.DTOs.RoomFacilityDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interface
{
    public interface IRoomManagmentService
    {
        Task<ResponseDto<Guid>> CreateRoomAsync(CreateRoomDto dto);
        Task<ResponseDto<Guid>> CreateFacilityAsync(CreateFacilityDto dto);
        Task<ResponseDto<bool>> AssignFacilityToRoomAsync(AssignFacilityToRoomDto dto);
        Task<ResponseDto<List<RoomDto>>> GetAllRoomsAsync();
        Task<ResponseDto<RoomDto>> GetRoomByIdAsync(Guid id);
        Task<ResponseDto<bool>> UpdateRoomAsync(UpdateRoomDto dto);
        Task<ResponseDto<bool>> DeleteRoomAsync(Guid id);
        Task<ResponseDto<List<FacilityDto>>> GetAllFacilitiesAsync();
        Task<ResponseDto<FacilityDto>> GetFacilityByIdAsync(Guid id);
        Task<ResponseDto<bool>> UpdateFacilityAsync(UpdateFacilityDto dto);
        Task<ResponseDto<bool>> DeleteFacilityAsync(Guid id);
        Task<ResponseDto<List<RoomFacilityDto>>> GetFacilitiesByRoomIdAsync(Guid roomId);
        Task<ResponseDto<bool>> UnassignFacilityFromRoomAsync(Guid roomId, Guid facilityId);


    }
}
