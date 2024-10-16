using OfficePortal.Models.Admin_Models.Announcement_Models;

namespace OfficePortal.Models.Admin_Models
{
    public class AdminViewModel
    {
        public int form_requests_count { get; set; }

        public int comment_requests_count { get; set; }

        public int initative_requests_count { get; set; }

        public List<Announcement> announcements { get; set; }

        public AnnouncementViewModel announcement { get; set; }
    }
}
