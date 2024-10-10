namespace OfficePortal.Models
{
    public class Reply_Comment_Model
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime PostedDate { get; set; }
        public string status { get; set; }
        public int AnnouncementId { get; set; }
        public int Comment_Id { get; set; }

    }
}
