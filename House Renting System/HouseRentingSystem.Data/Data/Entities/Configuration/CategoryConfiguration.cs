using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Data.Data.Entities.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(SeedCategories());
        }

        private IEnumerable<Category> SeedCategories()
        {
            return new Category[]
            {
                new Category()
                {
                    Id = 1,
                    Name = "Cottage"
                },

                new Category()
                {
                    Id = 2,
                    Name = "Single-Family huose"
                },

                new Category()
                {
                    Id = 3,
                    Name = "Duplex"
                },

                new Category()
                {
                    Id = 4,
                    Name = "One Bedroom"
                },

                new Category()
                {
                    Id = 5,
                    Name = "Two Bedroom"
                }
            };
        }
    }
}