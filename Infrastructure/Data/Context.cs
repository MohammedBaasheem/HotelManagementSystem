using Application.Helpers;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class Context: DbContext
    {
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Feedback> FeedBacks { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<PaymentMethod> PaymentMethodes { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleFeature> RoleFeatures { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomFacility> RoomFacilities { get; set; }
        public DbSet<RoomOffer> RoomOffers { get; set; }
        public DbSet<RoomReservation> RoomReservations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

    }
}