using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HouseRentingSystem.Data.Data.DataConstants.House;

namespace HouseRentingSystem.Data.Data.Entities
{
    public class House
    {
        [Key]
        public int Id {  get; init; }
        [MaxLength(TitleMaxLength)]
        [MinLength(10)]
        [Required]

        public string Title { get; set; } = null!;
        [MaxLength(AddressMaxLength)]
        [MinLength(50)]
        [Required]

        public string Address {  get; set; } = null!;


        [MaxLength(DescribtionMaxLength)]
        [MinLength(50)]
        [Required]
        public string Description {  get; set; } = null!;

        [Required]

        public string ImageUrl { get; set; } = null!;

        [MaxLength(2000)]
        [Required]

        [Column(TypeName = "decimal(12, 3)")]
        public decimal PricePerMonth {  get; set; }


        public int CategoryId {  get; set; }

        public Category Category { get; set; } = null!;

        public int AgentId { get; set; }

        public Agent Agent {  get; set; }

        public string? RenterId {  get; set; }
        
        public ApplicationUser? Renter { get; set; }
    }
}
