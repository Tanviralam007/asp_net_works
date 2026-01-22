namespace ToolShare.API.DTOs.Transaction
{
    public class TransactionResponseDTO
    {
        public int Id { get; set; }
        public int BorrowRequestId { get; set; }
        public int? PaymentId { get; set; }
        public DateTime? HandoverDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime ExpectedReturnDate { get; set; }
        public int LateDays { get; set; }
        public decimal FineAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string ToolName { get; set; } = string.Empty;
        public string BorrowerName { get; set; } = string.Empty;
    }
}
