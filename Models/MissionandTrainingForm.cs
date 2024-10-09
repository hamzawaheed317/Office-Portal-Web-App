using Microsoft.VisualBasic;

namespace OfficePortal.Models
{
    public class MissionandTrainingForm
    {
        public int id { get; set; }
        public string EmployeeName { get; set; }

        public string JobTitle { get; set; }

        public string Grade { get; set; }

        public string Number { get; set; }

        public string Department { get; set; }

        public string LineManager { get; set; }

        public string Type { get; set; }

        public string Purpose { get; set; }

        public DateTime? Departure_date { get; set; }
        public DateTime? Arrival_date { get; set; }
        public DateTime? date_from { get; set; }
        public TimeSpan? Departure_time { get; set; }
        public TimeSpan? Arrival_time { get; set; }

        public string status { get; set; }
        public string Form_type { get; set; }
    }
}
