﻿namespace OfficePortal.Models.Account_Models
{
    public class UserInfo
    {
        public string email { get; set; }
        public string password { get; set; }
        public string Employee_Name { get; set; }
        public string Job_Title { get; set; }
        public string Grade { get; set; }
        public string Employee_Number { get; set; }
        public string Line_Manager { get; set; }
        public string Department { get; set; }
        public string role { get; set; }
        public string Admin_email { get; set; }

        public string USer_role { get; set; }

        public List<string> Attendies { get; set; }
    }
}
