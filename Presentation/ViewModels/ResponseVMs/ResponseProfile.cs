using Application.DTOs.ResponseDTOs;
using AutoMapper;

namespace Presentation.ViewModels.ResponseVMs
{
    public class ResponseProfile : Profile
    {
        public ResponseProfile()
        {
            CreateMap(typeof(ResponseDto<>), typeof(ResponseVM<>)).ReverseMap();
        }
    }
}
