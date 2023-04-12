
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Shop
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        [StringLength(100, ErrorMessage = "The Name field must be a string with a maximum length of {1}.", MinimumLength = 1)]
        public string Name { get; set; }

        [Required(ErrorMessage = "The CountProduct field is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The CountProduct field must be a positive integer.")]
        public int CountProduct { get; set; }

        [Required(ErrorMessage = "The PriceOfOne field is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "The PriceOfOne field must be a positive number.")]
        public double PriceOfOne { get; set; }

        [Required(ErrorMessage = "The DateOfSaving field is required.")]
        [DataType(DataType.Date, ErrorMessage = "The DateOfSaving field must be a valid date.")]
        public DateTime DateOfSaving { get; set; }
    }

}