using System;
using System.Collections.Generic;
using System.Text;

namespace Application.IRepositories
{
    public interface IOfferManagmentRepository: IGeneralRepository<Domain.Models.Offer>
    {
        Task UpdateOfferRoomsAsync(Guid offerId, List<Guid> roomIds);
    }
}
