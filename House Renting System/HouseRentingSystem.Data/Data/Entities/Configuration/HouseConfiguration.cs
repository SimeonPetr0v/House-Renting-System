using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Data.Data.Entities.Configuration
{
    public class HouseConfiguration : IEntityTypeConfiguration<House>
    {
        public void Configure(EntityTypeBuilder<House> builder)
        {
            builder.HasOne(h=> h.Category).WithMany(c => c.Houses).HasForeignKey(h=>h.CategoryId).OnDelete(DeleteBehavior.Restrict);
            //builder.HasData(SeedHouses());
        }

        private IEnumerable<House> SeedHouses()
        {
            return new House[]
            {
               new House
               {
                   Id = 1,
                   Title = "Rila House",
                   Address = "Rila Mountain",
                   Description = "Big house in the mountain",
                   ImageUrl = @"https://bghike.com/en/images/huts_pic/rila_lakes_main.jpg",
                   PricePerMonth = 1000.00M,
                   CategoryId = 2,
                   AgentId = 1,

               },
               new House
               {
                   Id = 1,
                   Title = "Modern House",
                   Address = "Miami",
                   Description = "Large modern house",
                   ImageUrl = @"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQvcje9el9YUxSqN4VSt3llpb6su9ghN-_ZbA&s",
                   PricePerMonth = 1200.00M,
                   CategoryId = 1,
                   AgentId = 1,

               },

            };
        }
    }
}
