using System.ComponentModel.DataAnnotations;

namespace OfficePortal.Models
{
    public class EventModel
    {   
            public int Id { get; set; } // Primary Key

            [Required]
            public string Title { get; set; } // Event Title

            [Required]
            public DateTime?StartDate { get; set; } // Start date and time

            [Required]
            public DateTime? EndDate { get; set; } // End date and time

            public string? Repeat { get; set; } // Options: Don't Repeat, Daily, Weekly, Monthly, Yearly

            [Required]
            public string Location { get; set; } // Event location

            public string? Attendees { get; set; } // Comma-separated list of attendees

            public string? Description { get; set; } // Event description

            public string? Reminder { get; set; } // Reminder time options

            public string? Label { get; set; } // Event type: Event, Meeting, Holiday

        }
    }
