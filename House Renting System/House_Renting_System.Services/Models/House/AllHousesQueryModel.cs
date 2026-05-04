namespace House_Renting_System.Services.Models
{
    public enum HouseSorting
    {
        Newest = 0,
        Price = 1,
        NotRentedFirst = 2
    }

    public class AllHousesQueryModel
    {
        public const int HousesPerPage = 3;

        public string? Category { get; set; }

        public string? SearchTerm { get; set; }
        
        public HouseSorting Sorting { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int TotalHousesCount { get; set; }

        public IEnumerable<string> Categories { get; set; } = new List<string>();

        public IEnumerable<HouseViewModel> Houses { get; set; } = new List<HouseViewModel>();
    }
}