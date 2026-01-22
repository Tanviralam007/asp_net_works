namespace STFMS.API.DTOs.Analytics
{
    public class DashboardStatsResponseDTO
    {
        public int TotalBookings { get; set; }
        public int CompletedRides { get; set; }
        public int ActiveBookings { get; set; }
        public int PendingBookings { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalDrivers { get; set; }
        public int ActiveDrivers { get; set; }
        public int TotalVehicles { get; set; }
        public int ActiveVehicles { get; set; }
        public decimal AverageRating { get; set; }
    }
}
