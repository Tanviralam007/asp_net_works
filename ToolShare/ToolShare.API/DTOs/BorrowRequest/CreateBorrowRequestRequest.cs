using System.ComponentModel.DataAnnotations;

namespace ToolShare.API.DTOs.BorrowRequest
{
    public class CreateBorrowRequestRequest
    {
        [Required(ErrorMessage = "Tool ID is required")]
        public int ToolId { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }
    }
}
