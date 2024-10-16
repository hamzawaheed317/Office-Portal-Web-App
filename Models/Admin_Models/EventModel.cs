using System;
using System.ComponentModel.DataAnnotations;
using OfficePortal.ValidationAttributes;

namespace OfficePortal.Models.Admin_Models
{
    public class EventModel
    {
        public int Id { get; set; } // Primary Key

        [Required(ErrorMessage = "_localization.Getkey('ErrorTitleRequired')")]
        public string Title { get; set; } // Event Title

        [Required(ErrorMessage = "_localization.Getkey('ErrorStartDateRequired')")]
        public DateTime? StartDate { get; set; } // Start date and time

        [Required(ErrorMessage = "_localization.Getkey('ErrorEndDateRequired')")]
        [DateGreaterThan("StartDate")]
        public DateTime? EndDate { get; set; } // End date and time

        [Required(ErrorMessage = "_localization.Getkey('ErrorRepeatRequired')")]
        public string Repeat { get; set; } // Options: Don't Repeat, Daily, Weekly, Monthly, Yearly

        [Required(ErrorMessage = "_localization.Getkey('ErrorLocationRequired')")]
        public string Location { get; set; } // Event location

        [Required(ErrorMessage = "_localization.Getkey('ErrorAttendeesRequired')")]
        public string Attendees { get; set; } // Comma-separated list of attendees

        [Required(ErrorMessage = "_localization.Getkey('ErrorDescriptionRequired')")]
        public string Description { get; set; } // Event description

        [Required(ErrorMessage = "_localization.Getkey('ErrorReminderRequired')")]
        public string Reminder { get; set; } // Reminder time options

        [Required(ErrorMessage = "_localization.Getkey('ErrorLabelRequired')")]
        public string Label { get; set; } // Event type: Event, Meeting, Holiday
    }
}
