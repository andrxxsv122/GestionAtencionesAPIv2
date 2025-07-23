using Microsoft.AspNetCore.Mvc;
using GestionAtencionesAPI.Models;
using GestionAtencionesAPI.Repositories;
using GestionAtencionesAPI.DTO;
using System.Data.SqlClient;

namespace GestionAtencionesAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentRepository _repo;

    public AppointmentController(IAppointmentRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _repo.GetAllAsync();
        if (result == null || !result.Any())
            return NotFound(new { message = "No se encontraron atenciones registradas." });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var appointment = await _repo.GetByIdAsync(id);
        if (appointment == null)
            return NotFound(new { message = $"No se encontró una atención con ID {id}." });

        return Ok(appointment);
    }

    [HttpGet("date/{date}")]
    public async Task<IActionResult> GetByDate(DateTime date)
    {
        var result = await _repo.GetByDateAsync(date);
        if (result == null || !result.Any())
            return NotFound(new { message = $"No se encontraron atenciones para la fecha {date:yyyy-MM-dd}." });

        return Ok(result);
    }

    [HttpGet("doctor/{doctorId}")]
    public async Task<IActionResult> GetByDoctor(int doctorId)
    {
        var result = await _repo.GetByDoctorAsync(doctorId);
        if (result == null || !result.Any())
            return NotFound(new { message = $"No se encontraron atenciones para el doctor con ID {doctorId}." });

        return Ok(result);
    }

    [HttpGet("patient/{patientId}")]
    public async Task<IActionResult> GetByPatient(int patientId)
    {
        var result = await _repo.GetByPatientAsync(patientId);
        if (result == null || !result.Any())
            return NotFound(new { message = $"No se encontraron atenciones para el paciente con ID {patientId}." });

        return Ok(result);
    }

    [HttpGet("speciality/{specialityId}")]
    public async Task<IActionResult> GetBySpeciality(int specialityId)
    {
        var result = await _repo.GetBySpecialityAsync(specialityId);
        if (result == null || !result.Any())
            return NotFound(new { message = $"No se encontraron atenciones para la especialidad con ID {specialityId}." });

        return Ok(result);
    }

    [HttpGet("average-duration/{specialityId}")]
    public async Task<IActionResult> GetAverageDuration(int specialityId)
    {
        var avg = await _repo.GetDurationBySpecialityAsync(specialityId);

        if (avg == 0)
            return NotFound(new { message = $"No hay atenciones registradas para la especialidad con ID {specialityId}." });

        return Ok(new
        {
            message = $"Duración promedio calculada correctamente.",
            averageDurationInMinutes = avg
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AppointmentDTO dto)
    {
        var existsOverlap = await _repo.ExistsOverlapAsync(dto.DoctorId, dto.Appointment_StartUtc, dto.Appointment_EndUtc);
        if (existsOverlap)
            return Conflict(new { message = "Ya existe una atención en ese rango para el doctor." });

        var appointment = new Appointment
        {
            PatientId = dto.PatientId,
            DoctorId = dto.DoctorId,
            Appointment_StartUtc = dto.Appointment_StartUtc,
            Appointment_EndUtc = dto.Appointment_EndUtc,
            Appointment_Diagnosis = dto.Appointment_Diagnosis,
            Appointment_CreatedBy = "api",
            Appointment_CreatedAt = DateTime.Now
        };

        var newId = await _repo.CreateAsync(appointment);
        appointment.Appointment_Id = newId;

        return Ok(new
        {
            message = "Atención registrada exitosamente.",
            data = appointment
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] AppointmentDTO dto)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null)
            return NotFound(new { message = $"No se encontró una atención con ID {id}." });

        var appointment = new Appointment
        {
            Appointment_Id = id,
            PatientId = dto.PatientId,
            DoctorId = dto.DoctorId,
            Appointment_StartUtc = dto.Appointment_StartUtc,
            Appointment_EndUtc = dto.Appointment_EndUtc,
            Appointment_Diagnosis = dto.Appointment_Diagnosis,
            Appointment_CreatedBy = existing.Appointment_CreatedBy,
            Appointment_CreatedAt = existing.Appointment_CreatedAt,
            Appointment_ModifiedBy = "api",
            Appointment_ModifiedAt = DateTime.Now
        };

        var result = await _repo.UpdateAsync(appointment);
        if (!result)
            return StatusCode(500, new { message = "Error al actualizar la atención." });

        return Ok(new
        {
            message = "Atención actualizada correctamente.",
            data = appointment
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _repo.DeleteAsync(id);

        if (!deleted)
            return NotFound(new { message = $"No se encontró una atención con ID {id} para eliminar." });

        return Ok(new { message = $"Atención con ID {id} eliminada exitosamente." });
    }
}