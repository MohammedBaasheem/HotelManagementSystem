using AutoMapper;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.OfferDTOs
{
    public class OfferProfile : Profile
    {
        public OfferProfile()
        {

            CreateMap<CreateOfferDto, Offer>()
                .ForMember(dest => dest.RoomOffer, opt => opt.Ignore())
                .ForMember(dest => dest.Code, opt => opt.Ignore());



            CreateMap<UpdateOfferDto, Offer>()
                .ForMember(dest => dest.Code, opt => opt.Ignore())      
                .ForMember(dest => dest.RoomOffer, opt => opt.Ignore());



            CreateMap<Offer, GetOfferDto>()
                .ForMember(dest => dest.RoomIds,
                           opt => opt.MapFrom(src => src.RoomOffer.Select(x => x.RoomId)));
        }
    }
}
