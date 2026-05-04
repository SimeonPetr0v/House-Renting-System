namespace House_Renting_System.Services.Models
{
    public class HouseDetailViewModel : HouseViewModel
    {
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CreatedBy { get; set; }
    }
}