using System.ComponentModel.DataAnnotations;

namespace ToolShare.API.DTOs.Review
{
    public class CreateReviewRequest
    {
        [Required(ErrorMessage = "Tool ID is required")]
        public int ToolId { get; set; }

        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [StringLength(500)]
        public string? Comment { get; set; }

        [Required]
        [Range(0, 1, ErrorMessage = "Review type must be 0 (ToolReview) or 1 (UserReview)")]
        public byte ReviewType { get; set; }
    }
}
