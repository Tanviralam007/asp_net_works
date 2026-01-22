using System.ComponentModel.DataAnnotations;

namespace ToolShare.API.DTOs.Tool
{
    public class CreateToolRequest
    {
        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Tool name is required")]
        [StringLength(150, MinimumLength = 2)]
        public string ToolName { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Daily rate is required")]
        [Range(1, 100000, ErrorMessage = "Daily rate must be between 1 and 100000")]
        public decimal DailyRate { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [StringLength(200)]
        public string Location { get; set; } = string.Empty;
    }
}
