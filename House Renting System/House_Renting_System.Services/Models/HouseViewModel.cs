namespace House_Renting_System.Services.Models
{
    public class HouseViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

        public bool CurentUserIsOwner { get; set; }

        public decimal PricePerMonth { get; set; }

        public bool IsRented { get; set; }
    }
}