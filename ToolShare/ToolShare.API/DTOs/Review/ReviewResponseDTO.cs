namespace ToolShare.API.DTOs.Review
{
    public class ReviewResponseDTO
    {
        public int Id { get; set; }
        public int ToolId { get; set; }
        public string ToolName { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime ReviewDate { get; set; }
        public string ReviewType { get; set; } = string.Empty;
    }
}
