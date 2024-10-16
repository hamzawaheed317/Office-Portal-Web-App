using OfficePortal.Models.Employee_Models;

namespace OfficePortal.Models.Employee_Models
{
    public class SocialStatusForm
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string JobTitle { get; set; }
        public string Nationality { get; set; }
        public string Department { get; set; }
        public string BayanatiNumber { get; set; }
        public DateTime? JoiningDate { get; set; } 

        // Spouse Information
        public string SpouseFullName { get; set; }
        public string SpouseWorkplace { get; set; }
        public DateTime? SpouseDateOfMarriage { get; set; }
        public DateTime? SpouseDateOfEmployment { get; set; }

        // Children's Information
        public int? NumberOfChildren { get; set; }
        public string SonName { get; set; }

        // Change in Social Status
        public string ReasonForChange { get; set; }
        public DateTime? DateOfChange { get; set; }

        // Navigation property for related files
        public virtual ICollection<File> Files { get; set; } = new List<File>();

        public string Form_Type { get; set; }
        public string Status { get; set; }

    }
}
public class File
{
    public int Id { get; set; }
    public string File_Name { get; set; }
    public int SocialStatusRecord_Id { get; set; }

    public virtual SocialStatusForm SocialStatusRecord { get; set; }
}