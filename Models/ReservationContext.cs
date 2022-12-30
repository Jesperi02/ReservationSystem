using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace ReservationSystem22.Models
{
    public class ReservationContext:DbContext
    {
        public ReservationContext(DbContextOptions<ReservationContext> options)
            : base(options) { 
        }

        public DbSet<Item> Items { get; set; } = null!;
        public DbSet<Image> Images { get; set; } = null!;
        public DbSet<Reservation> Reservations { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
    }
}