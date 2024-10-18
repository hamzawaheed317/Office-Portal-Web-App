using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace OfficePortal.Models.Employee_Models
{
    public class SocialStatusFormViewModel
    {
        // Employee Information
        public string EmployeeName { get; set; }
        public string JobTitle { get; set; }
        public string Nationality { get; set; }
        public string Department { get; set; }
        public string BayanatiNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime? JoiningDate { get; set; }

        // Spouse Information
        [Required(ErrorMessage = "_localization.Getkey('ErrorSpouseFullNameRequired')")]
        public string SpouseFullName { get; set; }

        public string SpouseWorkplace { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "_localization.Getkey('ErrorSpouseDateOfMarriageRequired')")]
        public DateTime? SpouseDateOfMarriage { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "_localization.Getkey('ErrorSpouseDateOfEmploymentRequired')")]
        public DateTime? SpouseDateOfEmployment { get; set; }

        // Children's Information
        [Required(ErrorMessage = "_localization.Getkey('ErrorNumberOfChildrenRequired')")]
        public int? NumberOfChildren { get; set; }

        [Required(ErrorMessage = "_localization.Getkey('ErrorSonNameRequired')")]
        public string SonName { get; set; }

        // Change in Social Status
        [Required(ErrorMessage = "_localization.Getkey('ErrorReasonForChangeRequired')")]
        public string ReasonForChange { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "_localization.Getkey('ErrorDateOfChangeRequired')")]
        public DateTime? DateOfChange { get; set; }

        // File Uploads
        [Required(ErrorMessage = "_localization.Getkey('ErrorUploadedFilesRequired')")]
        public List<IFormFile> UploadedFiles { get; set; }

        public string Form_Type { get; set; }
        public string Status { get; set; }

    }
}
