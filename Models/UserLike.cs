namespace OfficePortal.Models
{
    public class UserLike
    {
        public int Id { get; set; }
        public int AnnouncementId { get; set; }
        public string UserId { get; set; } // Or however you identify users
    }
}
