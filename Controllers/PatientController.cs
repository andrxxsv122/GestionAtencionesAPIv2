using Microsoft.AspNetCore.Mvc;
using GestionAtencionesAPI.Models;
using Microsoft.AspNetCore.Http;
using GestionAtencionesAPI.Repositories;
using GestionAtencionesAPI.DTO;

/// <summary>
/// Controlador para gestionar pacientes.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PatientController : ControllerBase
{
    private readonly IPatientRepository _repository;
    private readonly IAppointmentRepository _appointmentRepo;

    public PatientController(IPatientRepository repository, IAppointmentRepository appointmentRepo)
    {
        _repository = repository;
        _appointmentRepo = appointmentRepo;
    }

    /// <summary>
    /// Obtiene todos los pacientes registrados.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var patients = await _repository.GetAllAsync();
        if (patients == null || !patients.Any())
            return NotFound(new { message = "No se encontraron pacientes registrados." });

        return Ok(patients);
    }

    /// <summary>
    /// Obtiene un paciente por su ID.
    /// </summary>
    /// <param name="id">ID del paciente</param>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var patient = await _repository.GetByIdAsync(id);
        if (patient == null)
            return NotFound(new { message = $"No se encontró un paciente con ID {id}." });

        return Ok(patient);
    }

    /// <summary>
    /// Obtiene un paciente por su RUT.
    /// </summary>
    /// <param name="rut">RUT del paciente</param>
    [HttpGet("rut/{rut}")]
    public async Task<IActionResult> GetByRut(string rut)
    {
        var patient = await _repository.GetByRutAsync(rut);
        if (patient == null)
            return NotFound(new { message = $"No se encontró un paciente con RUT '{rut}'." });

        return Ok(patient);
    }

    /// <summary>
    /// Registra un nuevo paciente.
    /// </summary>
    /// <param name="dto">Datos del paciente</param>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PatientDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Patient_FirstName))
            return BadRequest(new { message = "El nombre del paciente es obligatorio." });

        var existing = await _repository.GetByRutAsync(dto.Patient_RUT);
        if (existing != null)
            return Conflict(new { message = "Ya existe un paciente con ese RUT." });

        var patient = new Patient
        {
            Patient_FirstName = dto.Patient_FirstName,
            Patient_LastName = dto.Patient_LastName,
            Patient_RUT = dto.Patient_RUT,
            Patient_DateOfBirth = dto.Patient_DateOfBirth,
            Patient_Gender = dto.Patient_Gender,
            Patient_Phone = dto.Patient_Phone,
            Patient_Email = dto.Patient_Email,
            Patient_AddressLine1 = dto.Patient_AddressLine1,
            Patient_AddressLine2 = dto.Patient_AddressLine2,
            Patient_City = dto.Patient_City,
            Patient_State = dto.Patient_State,
            Patient_PostalCode = dto.Patient_PostalCode,
            Patient_CreatedBy = "api",
            Patient_CreatedAt = DateTime.Now
        };

        var id = await _repository.CreateAsync(patient);
        patient.Patient_Id = id;

        return Ok(new
        {
            message = "Paciente registrado correctamente.",
            data = patient
        });
    }

    /// <summary>
    /// Actualiza los datos de un paciente existente.
    /// </summary>
    /// <param name="id">ID del paciente a actualizar</param>
    /// <param name="dto">Nuevos datos del paciente</param>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] PatientDTO dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
            return NotFound(new { message = $"No se encontró un paciente con ID {id}." });

        var patient = new Patient
        {
            Patient_Id = id,
            Patient_FirstName = dto.Patient_FirstName,
            Patient_LastName = dto.Patient_LastName,
            Patient_RUT = dto.Patient_RUT,
            Patient_DateOfBirth = dto.Patient_DateOfBirth,
            Patient_Gender = dto.Patient_Gender,
            Patient_Phone = dto.Patient_Phone,
            Patient_Email = dto.Patient_Email,
            Patient_AddressLine1 = dto.Patient_AddressLine1,
            Patient_AddressLine2 = dto.Patient_AddressLine2,
            Patient_City = dto.Patient_City,
            Patient_State = dto.Patient_State,
            Patient_PostalCode = dto.Patient_PostalCode,
            Patient_CreatedBy = existing.Patient_CreatedBy,
            Patient_CreatedAt = existing.Patient_CreatedAt,
            Patient_ModifiedBy = "api",
            Patient_ModifiedAt = DateTime.Now
        };

        var result = await _repository.UpdateAsync(patient);
        if (!result)
            return StatusCode(500, new { message = "Error al actualizar el paciente." });

        return Ok(new
        {
            message = "Paciente actualizado correctamente.",
            data = patient
        });
    }

    /// <summary>
    /// Elimina un paciente por su ID (si no tiene atenciones registradas).
    /// </summary>
    /// <param name="id">ID del paciente a eliminar</param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var hasAppointments = await _appointmentRepo.ExistAppointForPatientAsync(id);
        if (hasAppointments)
            return Conflict(new { message = $"No se puede eliminar el paciente con ID {id} porque tiene atenciones registradas." });

        var deleted = await _repository.DeleteAsync(id);
        if (!deleted)
            return NotFound(new { message = $"No se encontró un paciente con ID {id}." });

        return Ok(new { message = $"Paciente con ID {id} eliminado/a exitosamente." });
    }
}
