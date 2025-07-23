using Dapper;
using GestionAtencionesAPI.Data;
using GestionAtencionesAPI.Models;

namespace GestionAtencionesAPI.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly DbContext _db;

    public AppointmentRepository(DbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Appointment>> GetAllAsync()
    {
        var query = "SELECT * FROM Appointment";
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<Appointment>(query);
    }

    public async Task<Appointment?> GetByIdAsync(int id)
    {
        var query = "SELECT * FROM Appointment WHERE Appointment_Id = @Id";
        using var conn = _db.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Appointment>(query, new { Id = id });
    }

    public async Task<IEnumerable<Appointment>> GetByDateAsync(DateTime date)
    {
        var query = "SELECT * FROM Appointment WHERE CAST(Appointment_StartUtc AS DATE) = @Date";
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<Appointment>(query, new { Date = date.Date });
    }

    public async Task<IEnumerable<Appointment>> GetByDoctorAsync(int doctorId)
    {
        var query = "SELECT * FROM Appointment WHERE DoctorId = @DoctorId";
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<Appointment>(query, new { DoctorId = doctorId });
    }

    public async Task<IEnumerable<Appointment>> GetByPatientAsync(int patientId)
    {
        var query = "SELECT * FROM Appointment WHERE PatientId = @PatientId";
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<Appointment>(query, new { PatientId = patientId });
    }

    public async Task<IEnumerable<Appointment>> GetBySpecialityAsync(int specialityId)
    {
        var query = @"
            SELECT a.* FROM Appointment a
            INNER JOIN Doctor d ON a.DoctorId = d.Doctor_Id
            WHERE d.SpecialityId = @SpecialityId";
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<Appointment>(query, new { SpecialityId = specialityId });
    }

    public async Task<double> GetDurationBySpecialityAsync(int specialityId)
    {
        var query = @"
            SELECT AVG(DATEDIFF(MINUTE, a.Appointment_StartUtc, a.Appointment_EndUtc)) AS AvgDuration
            FROM Appointment a
            INNER JOIN Doctor d ON a.DoctorId = d.Doctor_Id
            WHERE d.SpecialityId = @SpecialityId";
        using var conn = _db.CreateConnection();
        return await conn.ExecuteScalarAsync<double>(query, new { SpecialityId = specialityId });
    }

    public async Task<bool> ExistsOverlapAsync(int doctorId, DateTime start, DateTime end)
    {
        var query = @"
            SELECT COUNT(*) FROM Appointment
            WHERE DoctorId = @DoctorId
              AND (
                    (@Start < Appointment_EndUtc AND @End > Appointment_StartUtc)
                  )";
        using var conn = _db.CreateConnection();
        var count = await conn.ExecuteScalarAsync<int>(query, new { DoctorId = doctorId, Start = start, End = end });
        return count > 0;
    }
    public async Task<bool> ExistAppointForPatientAsync(int patientId)
    {
        var query = "SELECT COUNT(1) FROM Appointment WHERE PatientId = @PatientId";
        using var conn = _db.CreateConnection();
        var count = await conn.ExecuteScalarAsync<int>(query, new { PatientId = patientId });
        return count > 0;
    }

    public async Task<bool> ExistAppointForDoctorAsync(int doctorId)
    {
        var query = "SELECT COUNT(1) FROM Appointment WHERE DoctorId = @DoctorId";
        using var conn = _db.CreateConnection();
        var count = await conn.ExecuteScalarAsync<int>(query, new { DoctorId = doctorId });
        return count > 0;
    }

    public async Task<int> CreateAsync(Appointment appointment)
    {
        var query = @"
            INSERT INTO Appointment (
                PatientId, DoctorId, Appointment_StartUtc, Appointment_EndUtc,
                Appointment_Diagnosis, Appointment_CreatedBy, Appointment_CreatedAt
            ) VALUES (
                @PatientId, @DoctorId, @Appointment_StartUtc, @Appointment_EndUtc,
                @Appointment_Diagnosis, @Appointment_CreatedBy, @Appointment_CreatedAt
            );
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        using var conn = _db.CreateConnection();
        return await conn.ExecuteScalarAsync<int>(query, appointment);
    }

    public async Task<bool> UpdateAsync(Appointment appointment)
    {
        var query = @"
            UPDATE Appointment SET
                PatientId = @PatientId,
                DoctorId = @DoctorId,
                Appointment_StartUtc = @Appointment_StartUtc,
                Appointment_EndUtc = @Appointment_EndUtc,
                Appointment_Diagnosis = @Appointment_Diagnosis,
                Appointment_ModifiedBy = @Appointment_ModifiedBy,
                Appointment_ModifiedAt = SYSDATETIME()
            WHERE Appointment_Id = @Appointment_Id";

        using var conn = _db.CreateConnection();
        return await conn.ExecuteAsync(query, appointment) > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query = "DELETE FROM Appointment WHERE Appointment_Id = @Id";
        using var conn = _db.CreateConnection();
        return await conn.ExecuteAsync(query, new { Id = id }) > 0;
    }
}
