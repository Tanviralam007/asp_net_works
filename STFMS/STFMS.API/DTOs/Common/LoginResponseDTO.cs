namespace STFMS.API.DTOs.Common
{
    public class LoginResponseDTO
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;
        //public string Token { get; set; } = string.Empty;
        public DateTime LoginTime { get; set; }
    }
}
