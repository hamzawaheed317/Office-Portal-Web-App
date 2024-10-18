namespace OfficePortal.Models
{
    public class CommentViewModel
    {
        public Comment Comment { get; set; }
        public List<Reply_Comment_Model> Replies { get; set; }
    }
}
