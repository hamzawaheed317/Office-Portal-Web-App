namespace OfficePortal.Models.Account_Models
{
    public class EmailSettings
    {
        public string SMTPServer { get; set; }
        public int SMTPPort { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public string EmailPassword { get; set; }
    }
}
