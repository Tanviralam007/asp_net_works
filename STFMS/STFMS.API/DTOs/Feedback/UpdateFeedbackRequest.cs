using System.ComponentModel.DataAnnotations;

namespace STFMS.API.DTOs.Feedback
{
    public class UpdateFeedbackRequest
    {
        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [StringLength(500, ErrorMessage = "Comments cannot exceed 500 characters")]
        public string? Comments { get; set; }
    }
}
