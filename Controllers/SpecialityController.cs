using Microsoft.AspNetCore.Mvc;
using GestionAtencionesAPI.Models;
using GestionAtencionesAPI.Repositories;
using GestionAtencionesAPI.DTO;

namespace GestionAtencionesAPI.Controllers;

/// <summary>
/// Controlador para gestionar especialidades médicas.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SpecialityController : ControllerBase
{
    private readonly ISpecialityRepository _repo;
    private readonly IDoctorRepository _doctorRepo;

    public SpecialityController(ISpecialityRepository repo, IDoctorRepository doctorRepo)
    {
        _repo = repo;
        _doctorRepo = doctorRepo;
    }

    /// <summary>
    /// Obtiene todas las especialidades registradas.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _repo.GetAllAsync();
        if (result == null || !result.Any())
            return NotFound(new { message = "No se encontraron especialidades registradas." });

        return Ok(result);
    }

    /// <summary>
    /// Obtiene una especialidad por su ID.
    /// </summary>
    /// <param name="id">ID de la especialidad</param>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var speciality = await _repo.GetByIdAsync(id);
        if (speciality == null)
            return NotFound(new { message = $"No se encontró una especialidad con ID {id}." });

        return Ok(speciality);
    }

    /// <summary>
    /// Registra una nueva especialidad médica.
    /// </summary>
    /// <param name="dto">Datos de la especialidad</param>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SpecialityDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Speciality_Name))
            return BadRequest(new { message = "El nombre de la especialidad es obligatorio." });

        var existing = await _repo.GetByNameAsync(dto.Speciality_Name);
        if (existing != null)
            return Conflict(new { message = $"Ya existe una especialidad con el nombre '{dto.Speciality_Name}'." });

        var speciality = new Speciality
        {
            Speciality_Name = dto.Speciality_Name,
            Speciality_Description = dto.Speciality_Description,
            Speciality_CreatedBy = "api",
            Speciality_CreatedAt = DateTime.Now
        };

        var newId = await _repo.CreateAsync(speciality);
        speciality.Speciality_Id = newId;

        return Ok(new
        {
            message = "Especialidad creada correctamente.",
            data = speciality
        });
    }

    /// <summary>
    /// Actualiza una especialidad existente por ID.
    /// </summary>
    /// <param name="id">ID de la especialidad a actualizar</param>
    /// <param name="dto">Nuevos datos de la especialidad</param>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] SpecialityDTO dto)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null)
            return NotFound(new { message = $"No se encontró una especialidad con ID {id}." });

        var speciality = new Speciality
        {
            Speciality_Id = id,
            Speciality_Name = dto.Speciality_Name,
            Speciality_Description = dto.Speciality_Description,
            Speciality_CreatedBy = existing.Speciality_CreatedBy,
            Speciality_CreatedAt = existing.Speciality_CreatedAt,
            Speciality_ModifiedBy = "api",
            Speciality_ModifiedAt = DateTime.Now
        };

        var result = await _repo.UpdateAsync(speciality);
        if (!result)
            return StatusCode(500, new { message = "Ocurrió un error al actualizar la especialidad." });

        return Ok(new
        {
            message = "Especialidad actualizada correctamente.",
            data = speciality
        });
    }

    /// <summary>
    /// Elimina una especialidad por su ID (si no está asociada a doctores).
    /// </summary>
    /// <param name="id">ID de la especialidad a eliminar</param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var linkedDoctors = await _doctorRepo.GetBySpecialityIdAsync(id);
        if (linkedDoctors.Any())
        {
            return Conflict(new
            {
                message = $"No se puede eliminar la especialidad con ID {id} porque está asociada a uno o más doctores."
            });
        }

        var deleted = await _repo.DeleteAsync(id);
        if (!deleted)
            return NotFound(new { message = $"No se encontró una especialidad con ID {id}." });

        return Ok(new { message = $"Especialidad con ID {id} eliminada exitosamente." });
    }
}
