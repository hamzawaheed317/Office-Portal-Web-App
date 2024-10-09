namespace OfficePortal.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime PostedDate { get; set; }

        public string status { get; set; }
        public int AnnouncementId { get; set; } // Foreign key to Announcement
    }
}
