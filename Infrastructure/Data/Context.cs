using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class Context: DbContext
    {
        DbSet<Facility> Facilities { get; set; }
        DbSet<Feature> Features { get; set; }
        DbSet<Feedback> FeedBacks { get; set; }
        DbSet<Invoice> Invoices { get; set; }
        DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        DbSet<Offer> Offers { get; set; }
        DbSet<PaymentMethod> PaymentMethodes { get; set; }
        DbSet<Reservation> Reservations { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<RoleFeature> RoleFeatures { get; set; }
        DbSet<Room> Rooms { get; set; }
        DbSet<RoomFacility> RoomFacilities { get; set; }
        DbSet<RoomOffer> RoomOffers { get; set; }
        DbSet<RoomReservation> RoomReservations { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<UserRole> UserRoles { get; set; }
    }
}