using System;

namespace work_01_Client.DTOs
{
    public class AppointmentViewModel
    {
        public string? PatientName { get; set; }
        public string? Phone { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}
