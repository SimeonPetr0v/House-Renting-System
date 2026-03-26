using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HouseRentingSystem.Data.Data.DataConstants.Agent;

namespace HouseRentingSystem.Data.Data.Entities
{
    public class Agent
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = null!;

        public ApplicationUser User { get; init; } = null!;

        public IEnumerable<House> ManagedHouses { get; set; } = new List<House>();
    }
}
