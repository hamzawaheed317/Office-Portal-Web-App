namespace OfficePortal.Models.Admin_Models.Announcement_Models
{
    public class Announcement
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }

        public DateTime PostedDate { get; set; }
        public bool IsPinned { get; set; }
        public int LikesCount { get; set; }

        public string FileUrl { get; set; }
    }
}
