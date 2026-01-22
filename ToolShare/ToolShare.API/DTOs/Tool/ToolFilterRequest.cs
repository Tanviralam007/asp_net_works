namespace ToolShare.API.DTOs.Tool
{
    public class ToolFilterRequest
    {
        public int? CategoryId { get; set; }
        public string? Location { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? IsAvailable { get; set; }
    }
}
