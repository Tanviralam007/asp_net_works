namespace STFMS.API.DTOs.Analytics
{
    public class RevenueReportResponseDTO
    {
        public DateTime Date { get; set; }
        public decimal Revenue { get; set; }
        public int BookingCount { get; set; }
    }
}
