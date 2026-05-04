using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace work_01.DTOs
{
    public class AppointmentDTO
    {
        public string? PatientName { get; set; }
        [Required, StringLength(70)]

        public string? Phone { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime AppointmentDate { get; set; }
    }
}
