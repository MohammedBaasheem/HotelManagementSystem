using Application.DTOs.ResponseDTOs;
using AutoMapper;

namespace Presentation.ViewModels.ResponseVMs
{
    public class ResponseVMProfile:Profile
    {
        public ResponseVMProfile()
        {
            CreateMap(typeof(ResponseDto<>), typeof(ResponseVM<>));
        }
    }
}
