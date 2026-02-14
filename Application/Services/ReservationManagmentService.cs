using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.ResrvationDTOs;
using Application.Helpers;
using Application.IRepositories;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class ReservationManagmentService(
        IGeneralRepository<Reservation> reservationRepository,
        IGeneralRepository<Room> roomRepository)
    {

        public async Task<bool> AddReservation(AddResrvationDTO dto, Guid currentUserId)
        {
            var reservation = dto.MapOne<Reservation>();

            // Audit Properties
            reservation.CreatedBy = currentUserId;
            reservation.UpdatedBy = currentUserId;
            reservation.CreatedAt = DateTime.UtcNow; // Better to use UtcNow
            reservation.UpdatedAt = DateTime.UtcNow;

            // Business Logic (Nights & Prices)
            var nights = (short)(reservation.CheckOutDate - reservation.CheckInDate).Days;
            
            foreach (var roomRes in reservation.RoomReservations)
            {
                var room = await roomRepository.GetById(roomRes.RoomId);
                roomRes.numberOfNights = nights;
                roomRes.pricePerNight = room.PricePerNight;
                roomRes.totalPrice = room.PricePerNight * nights;
                
                // Track children audit too
                roomRes.CreatedBy = currentUserId;
                roomRes.CreatedAt = DateTime.UtcNow;
            }

            reservationRepository.Add(reservation);
            await reservationRepository.SaveChanges();
            return true;
        }

        public async Task<bool> UpdateReservation(UpdateReservationDTO dto, Guid currentUserId)
        {
            var reservation = await reservationRepository.GetWithTrackingById(dto.Id);
            if (reservation is null) return false;

            dto.MapOne(reservation);

            // Update Audit Properties
            reservation.UpdatedBy = currentUserId;
            reservation.UpdatedAt = DateTime.UtcNow;

            reservationRepository.Update(reservation);
            await reservationRepository.SaveChanges();
            return true;
        }
        public async Task<List<GetAllReservationsDTO>> GetAll()
        {
            // 1. Get the IQueryable (This is just a "promise" of a query, it hasn't hit the DB yet)
            var query = reservationRepository.GetAll();
            
            // 2. Project to DTO using your MapperHelper
            var projectedQuery = query.Map<GetAllReservationsDTO>();

            // 3. Execute the query Asynchronously
            // This is where the database is actually called
            return await projectedQuery.ToListAsync(); 
        }

        public async Task<bool> DeleteReservation(Guid id)
        {
            var reservation = await reservationRepository.GetById(id);
            if (reservation is null) return false;
            
            reservationRepository.SoftDelete(reservation);
            await reservationRepository.SaveChanges();
            return true;
        }

    }
}