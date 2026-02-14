using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Models;

namespace Application.DTOs.ResrvationDTOs
{
    public class ReservationProfile : Profile
    {
        public ReservationProfile()
        { 
            CreateMap<AddResrvationDTO, Reservation>()
            .ForMember(dest => dest.RoomReservations, opt => opt.MapFrom(src => src.SelectedRooms));

            CreateMap<AddRoomToReservationDTO, RoomReservation>();

            // Map from Domain to DTO (for GET requests)
            CreateMap<Reservation, GetAllReservationsDTO>();

            CreateMap<UpdateReservationDTO, Reservation>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
         }
    }
}