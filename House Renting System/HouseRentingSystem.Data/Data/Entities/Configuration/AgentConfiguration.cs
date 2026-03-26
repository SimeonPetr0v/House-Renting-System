using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Data.Data.Entities.Configuration
{
    public class AgentConfiguration : IEntityTypeConfiguration<Agent>
    {
        public void Configure(EntityTypeBuilder<Agent> builder)
        {
            //builder.HasData(SeedAgents());
        }

        private IEnumerable<Agent> SeedAgents()
        {
            return new Agent[]
            {
               new Agent
               {
                   Id = 1,
                   PhoneNumber = "123123123123123123",
                   UserId = "Abdul123"
               },
               new Agent
               {
                   Id = 2,
                   PhoneNumber = "2342342342334234234",
                   UserId = "Osama9/11"
               },

            };

        }
    }
}
