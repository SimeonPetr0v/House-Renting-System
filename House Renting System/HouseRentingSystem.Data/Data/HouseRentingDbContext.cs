using HouseRentingSystem.Data.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HouseRentingSystem.Data.Data.Entities.Configuration;

namespace HouseRentingSystem.Data.Data
{
    public class HouseRentingDbContext : IdentityDbContext<ApplicationUser>
    {
        public HouseRentingDbContext(DbContextOptions<HouseRentingDbContext> options) : base(options)
        {
        }
        public DbSet<House> Houses { get; init; } = null!;

        public DbSet<Category> Categories { get; init; } = null!;

        public DbSet<Agent> Agents { get; init; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new HouseConfiguration());
            builder.ApplyConfiguration(new AgentConfiguration());
            base.OnModelCreating(builder);
        }
    }
    }




