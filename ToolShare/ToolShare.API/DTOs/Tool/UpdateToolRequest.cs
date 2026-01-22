using System.ComponentModel.DataAnnotations;

namespace ToolShare.API.DTOs.Tool
{
    public class UpdateToolRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 2)]
        public string ToolName { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        [Range(1, 100000)]
        public decimal DailyRate { get; set; }

        [Required]
        [StringLength(200)]
        public string Location { get; set; } = string.Empty;

        public bool IsAvailable { get; set; }
    }
}
