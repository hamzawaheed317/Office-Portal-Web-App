using System;
using System.ComponentModel.DataAnnotations;

namespace OfficePortal.Models
{
    public class TrainingRequestViewModel
    {
        public int id { get; set; }

        public string EmployeeName { get; set; }

        public string JobTitle { get; set; }

        public string Grade { get; set; }

        public string Number { get; set; }

        public string Department { get; set; }

        public string LineManager { get; set; }

        [Required(ErrorMessage = "ErrorTypeRequired")]
        public string Type { get; set; }

        [Required(ErrorMessage = "ErrorTrainingEntityRequired")]
        public string TrainingEntity { get; set; }

        [Required(ErrorMessage = "ErrorDateFromRequired")]
        public DateTime? DateFrom { get; set; }

        [Required(ErrorMessage = "ErrorDateToRequired")]
        public DateTime? DateTo { get; set; }

        public string Location { get; set; }

        [Range(0.0, Double.MaxValue, ErrorMessage = "ErrorAmountRequired")]
        public double? Amount { get; set; }

        public string form_type { get; set; }

        public string status { get; set; }
    }
}
