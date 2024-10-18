using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OfficePortal.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime PostedDate { get; set; }
        public string status { get; set; }
        public int AnnouncementId { get; set; }



   }
}