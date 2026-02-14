using Application.IRepositories;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class OfferManagmentRepository:GeneralRepository<Offer>,IOfferManagmentRepository
    {
        public OfferManagmentRepository(Context context) : base(context)
        {
        }
        public async Task UpdateOfferRoomsAsync(Guid offerId, List<Guid> roomIds)
        {
           
            await _context.Set<RoomOffer>()
                .Where(x => x.OfferId == offerId)
                .ExecuteDeleteAsync();

            
            if (roomIds != null && roomIds.Any())
            {
                var newRelations = roomIds.Select(roomId => new RoomOffer
                {
                    OfferId = offerId,
                    RoomId = roomId
                });

                await _context.Set<RoomOffer>().AddRangeAsync(newRelations);
            }
        }
    }
}
