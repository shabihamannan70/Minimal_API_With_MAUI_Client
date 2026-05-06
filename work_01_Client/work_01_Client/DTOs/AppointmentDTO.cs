using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace work_01_Client.DTOs
{
    public class AppointmentDTO
    {
        public string? PatientName { get; set; }
        public string? Phone { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}
