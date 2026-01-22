namespace STFMS.API.DTOs.Analytics
{
    public class PopularRouteResponseDTO
    {
        public string PickupLocation { get; set; } = string.Empty;
        public string DropoffLocation { get; set; } = string.Empty;
        public int TripCount { get; set; }
        public decimal AverageFare { get; set; }
    }
}
