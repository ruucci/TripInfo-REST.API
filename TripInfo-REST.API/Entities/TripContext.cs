using System;
using Microsoft.EntityFrameworkCore;

namespace TripInfoREST.API.Entities
{
    public class TripContext : DbContext
    {
        public TripContext(DbContextOptions<TripContext> options)
           : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Attraction> Attractions { get; set; }
        public DbSet<User> Users { get; set; }
        //public DbSet<TokenRequest> TokenRequests { get; set; }
    }
}