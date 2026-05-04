using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace work_01.DTOs
{
    public class DoctorDTO
    {
        public int DoctorId { get; set; }
        [Required, StringLength(50)]
        public string? DoctorName { get; set; }
        [StringLength(250)]

        public string? Picture { get; set; }
        [Required, Column(TypeName = "decimal(18,2)")]

        public decimal VisitingFee { get; set; }
        public bool InDhaka { get; set; }
        public string? AppointmentJson { get; set; }
        public string? PictureBase64 { get; set; }
    }
}
