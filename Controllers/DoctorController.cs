using Microsoft.AspNetCore.Mvc;
using GestionAtencionesAPI.Models;
using GestionAtencionesAPI.Repositories;
using GestionAtencionesAPI.DTO;

namespace GestionAtencionesAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorController : ControllerBase
{
    private readonly IDoctorRepository _repo;
    private readonly IAppointmentRepository _appointmentRepo;

    public DoctorController(IDoctorRepository repo, IAppointmentRepository appointmentRepo)
    {
        _repo = repo;
        _appointmentRepo = appointmentRepo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _repo.GetAllAsync();
        if (result == null || !result.Any())
            return NotFound(new { message = "No se encontraron doctores registrados." });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var doctor = await _repo.GetByIdAsync(id);
        if (doctor == null)
            return NotFound(new { message = $"No se encontró un doctor con ID {id}." });

        return Ok(doctor);
    }

    [HttpGet("license/{license}")]
    public async Task<IActionResult> GetByLicense(string license)
    {
        var doctor = await _repo.GetByLicenseAsync(license);
        if (doctor == null)
            return NotFound(new { message = $"No se encontró un doctor con número de licencia '{license}'." });

        return Ok(doctor);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DoctorDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Doctor_FirstName) || string.IsNullOrWhiteSpace(dto.Doctor_LastName))
            return BadRequest(new { message = "Nombre y apellido son requeridos." });

        if (string.IsNullOrWhiteSpace(dto.Doctor_LicenseNumber))
            return BadRequest(new { message = "LicenseNumber es obligatorio." });

        var existing = await _repo.GetByLicenseAsync(dto.Doctor_LicenseNumber);
        if (existing != null)
            return Conflict(new { message = "Ya existe un doctor con ese número de licencia." });

        var doctor = new Doctor
        {
            Doctor_FirstName = dto.Doctor_FirstName,
            Doctor_LastName = dto.Doctor_LastName,
            Doctor_Email = dto.Doctor_Email,
            Doctor_Phone = dto.Doctor_Phone,
            Doctor_LicenseNumber = dto.Doctor_LicenseNumber,
            SpecialityId = dto.SpecialityId,
            Doctor_CreatedBy = "api",
            Doctor_CreatedAt = DateTime.Now
        };

        var id = await _repo.CreateAsync(doctor);
        doctor.Doctor_Id = id;

        return Ok(new
        {
            message = "Doctor registrado exitosamente.",
            data = doctor
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] DoctorDTO dto)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null)
            return NotFound(new { message = $"No se encontró un doctor con ID {id}." });

        var doctor = new Doctor
        {
            Doctor_Id = id,
            Doctor_FirstName = dto.Doctor_FirstName,
            Doctor_LastName = dto.Doctor_LastName,
            Doctor_Email = dto.Doctor_Email,
            Doctor_Phone = dto.Doctor_Phone,
            Doctor_LicenseNumber = dto.Doctor_LicenseNumber,
            SpecialityId = dto.SpecialityId,
            Doctor_CreatedBy = existing.Doctor_CreatedBy,
            Doctor_CreatedAt = existing.Doctor_CreatedAt,
            Doctor_ModifiedBy = "api",
            Doctor_ModifiedAt = DateTime.Now
        };

        var result = await _repo.UpdateAsync(doctor);
        if (!result)
            return StatusCode(500, new { message = "Error al actualizar el doctor." });

        return Ok(new
        {
            message = "Doctor actualizado correctamente.",
            data = doctor
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var hasAppointments = await _appointmentRepo.ExistAppointForDoctorAsync(id);
        if (hasAppointments)
            return Conflict(new { message = $"No se puede eliminar el doctor con ID {id} porque tiene atenciones registradas." });

        var deleted = await _repo.DeleteAsync(id);
        if (!deleted)
            return NotFound(new { message = $"No se encontró un doctor con ID {id} para eliminar." });

        return Ok(new { message = $"Doctor con ID {id} eliminado exitosamente." });
    }
}