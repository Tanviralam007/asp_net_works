namespace ToolShare.API.DTOs.BorrowRequest
{
    public class BorrowRequestResponseDTO
    {
        public int Id { get; set; }
        public int ToolId { get; set; }
        public string ToolName { get; set; } = string.Empty;
        public int BorrowerId { get; set; }
        public string BorrowerName { get; set; } = string.Empty;
        public DateTime RequestDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? ApprovalDate { get; set; }
        public decimal EstimatedCost { get; set; }
    }
}
