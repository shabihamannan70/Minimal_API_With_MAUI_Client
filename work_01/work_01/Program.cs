using Microsoft.EntityFrameworkCore;
using work_01.Models;
using Microsoft.AspNetCore.Mvc;
using work_01.DTOs;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<HospitalDbContext>(o=>o.UseSqlServer(builder.Configuration.GetConnectionString("appCon")));

builder.Services.AddControllers().AddJsonOptions(o=>o.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyAngularPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200") 
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("MyAngularPolicy");
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.MapGet("/api/doctors", async ([FromServices] HospitalDbContext db) =>     //using Microsoft.AspNetCore.Mvc; theke ashe [FromServices] [FromServices] HospitalDbContext db: এটি ASP.NET Core-এর Dependency Injection সিস্টেম ব্যবহার করে আপনার ডাটাবেস কন্টেক্সট (HospitalDbContext) কে সরাসরি এই মেথডের ভেতরে নিয়ে আসছে।
{
    return await db.Doctors.Include(x => 
x.Appointments).ToListAsync();
}).WithName("GetDoctors").Produces<Doctor[]>(StatusCodes.Status200OK);

app.MapGet("/api/doctors/{id}", async ([FromRoute] int id, [FromServices] HospitalDbContext db) =>
{
    var doctor=await db.Doctors.Include(x => x.Appointments).FirstOrDefaultAsync(x => x.DoctorId == id);
    return doctor is null ? Results.NotFound() : Results.Ok(doctor);
}).WithName("GetDoctor").Produces<Doctor>(StatusCodes.Status200OK);

app.MapPost("/api/doctors", async([FromBody] DoctorDTO doctorDTO, [FromServices] HospitalDbContext db)=>
{
    try
    {
        string picture = null;
        if (!string.IsNullOrEmpty(doctorDTO.PictureBase64))
        {
            picture = doctorDTO.PictureBase64;
        }
        var appointmentsList = new List<Appointment>();
        if (!string.IsNullOrWhiteSpace(doctorDTO.AppointmentJson))
        {
            var appointmentDTOs = JsonSerializer.Deserialize<List<AppointmentDTO>>(doctorDTO.AppointmentJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (appointmentDTOs != null)
            {
                appointmentsList = appointmentDTOs.Select(a => new Appointment
                {
                    PatientName = a.PatientName,
                    Phone = a.Phone,
                    AppointmentDate = a.AppointmentDate
                }).ToList();
            }
        }

        var Doctor = new Doctor
        {
            DoctorName = doctorDTO.DoctorName,
            VisitingFee = doctorDTO.VisitingFee,
            Picture = picture,
            InDhaka = doctorDTO.InDhaka,
            Appointments = appointmentsList,
        };
        db.Doctors.Add(Doctor);
        await db.SaveChangesAsync();
        return Results.Created($"/api/doctors/{Doctor.DoctorId}", Doctor);
    }
    catch (Exception ex)
    {
        return Results.BadRequest($"{ex.Message}");
    }
}).Produces<Doctor>(StatusCodes.Status201Created);

app.Run();



