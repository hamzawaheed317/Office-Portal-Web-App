using System.ComponentModel.DataAnnotations;

namespace OfficePortal.Models.Admin_Models.Announcement_Models
{
    public class AnnouncementViewModel
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public string Author { get; set; }


        // The file upload property
        [Required]
        [Display(Name = "Upload File")]
        public IFormFile FileUrl { get; set; }
    }
}
