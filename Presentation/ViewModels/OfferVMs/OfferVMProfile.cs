using Application.DTOs.OfferDTOs;
using AutoMapper;
using Domain.Models;

namespace Presentation.ViewModels.OfferVMs
{
    public class OfferVMProfile:Profile
    {
        public OfferVMProfile()
        {
            CreateMap<CreateOfferVM, CreateOfferDto>(); 

            CreateMap<UpdateOfferVM, UpdateOfferDto>();


            CreateMap<GetOfferDto, GetOfferVM>();

        }
    }
}
