using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace work_01_Client.DTOs
{
    public class DoctorDTO
    {
        public int DoctorId { get; set; }
        public string? DoctorName { get; set; }
        public string? Picture { get; set; }
        public decimal VisitingFee { get; set; }
        public bool InDhaka { get; set; }
        public string? AppointmentJson { get; set; } // Appointment লিস্ট পাঠানোর জন্য
        public string? PictureBase64 { get; set; }
    }
}
