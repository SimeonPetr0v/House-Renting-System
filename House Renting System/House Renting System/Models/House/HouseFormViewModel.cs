using House_Renting_System.Models.House.Helpers;
using System.ComponentModel.DataAnnotations;

namespace House_Renting_System.Models.House
{
    public class HouseFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "Title must be between 10 and 100 characters")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Address is required")]
        [StringLength(150, MinimumLength = 30, ErrorMessage = "Address must be between 30 and 150 characters")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, MinimumLength = 50, ErrorMessage = "Description must be between 50 and 500 characters")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Image URL is required")]
        public string ImageUrl { get; set; } = null!;

        [Required(ErrorMessage = "Price per month is required")]
        [Range(0.01, 100000, ErrorMessage = "Price must be between 0.01 and 100,000")]
        [DataType(DataType.Currency)]
        public decimal PricePerMonth { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Category is required")]
        public int SelectedCategoryId { get; set; }

        public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
    }
}