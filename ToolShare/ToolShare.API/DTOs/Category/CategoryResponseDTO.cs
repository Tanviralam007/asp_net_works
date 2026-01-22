namespace ToolShare.API.DTOs.Category
{
    public class CategoryResponseDTO
    {
        public int Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
