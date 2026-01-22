namespace STFMS.API.DTOs.Feedback
{
    public class FeedbackResponseDTO
    {
        public int FeedbackId { get; set; }
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public string? Comments { get; set; }
        public DateTime SubmittedDate { get; set; }
    }
}
