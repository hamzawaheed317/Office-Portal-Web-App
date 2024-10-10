namespace OfficePortal.Models.Admin_Models
{
    public class AdminViewModel
    {

        public int Id { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }

        public DateTime PostedDate { get; set; }
        public bool IsPinned { get; set; }
        public int LikesCount { get; set; }

        public string FileUrl { get; set; }


        public int form_requests_count { get; set; }

        public int comment_requests_count { get; set; }

        public int initative_requests_count { get; set; }
    }
}
