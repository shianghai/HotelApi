using HotelApi.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelApi.Data
{
    public class HotelDbContext : IdentityDbContext
    {
        public HotelDbContext(DbContextOptions options): base(options)
        {
           
        }
        public DbSet<Country>  Countries {get; set;}

        public DbSet<Hotel> Hotels { get; set;}


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.Entity<Country>().HasData(
                new Country()
                {
                    Id = -1,
                    Name = "Ghana",
                    ShortName = "GH"
                },
                new Country()
                {
                    Id = -2,
                    Name = "United States Of America",
                    ShortName = "USA",

                },
                new Country()
                {
                    Id = -3,
                    Name = "Nigeria",
                    ShortName = "NAIJA",
                }
            );

            modelBuilder.Entity<Country>().HasData(
                new Hotel()
                {
                    CountryId = -1,
                    City = "Accra",
                    Name = "Kempisnki",
                    Address = "Accra Central",
                    Id = 1
                },
                new Hotel()
                {
                    CountryId = -2,
                    City = "Miami",
                    Name = "Miami Beach Hotel",
                    Address = "21 cocunut st, Miami",
                    Id = 2,

                }
            );
        }
    }
}
