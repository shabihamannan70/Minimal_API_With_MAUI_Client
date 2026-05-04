using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace work_01.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        [Required, StringLength(50)]
        public string? DoctorName { get; set; }
        [StringLength(250)]

        public string? Picture { get; set; }
        [Required, Column(TypeName = "decimal(18,2)")]

        public decimal VisitingFee { get; set; }
        public bool InDhaka { get; set; }
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
    public class Appointment
    {
        public int AppointmentId { get; set; }
        [Required, StringLength(50)]

        public string? PatientName { get; set; }
        [Required, StringLength(70)]

        public string? Phone { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime AppointmentDate { get; set; }
        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public virtual Doctor? Doctor { get; set; }

    }

    public class HospitalDbContext : DbContext
    {
        public HospitalDbContext(DbContextOptions<HospitalDbContext> options) : base(options)
        {
        }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
    }

}
