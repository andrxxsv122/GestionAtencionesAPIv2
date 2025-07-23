using Dapper;
using GestionAtencionesAPI.Data;
using GestionAtencionesAPI.Models;
using GestionAtencionesAPI.Repositories;

public class DoctorRepository : IDoctorRepository
{
    private readonly DbContext _db;

    public DoctorRepository(DbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Doctor>> GetAllAsync()
    {
        var query = "SELECT * FROM Doctor";
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<Doctor>(query);
    }

    public async Task<Doctor?> GetByIdAsync(int id)
    {
        var query = "SELECT * FROM Doctor WHERE Doctor_Id = @Id";
        using var conn = _db.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Doctor>(query, new { Id = id });
    }

    public async Task<Doctor?> GetByLicenseAsync(string licenseNumber)
    {
        var query = "SELECT * FROM Doctor WHERE Doctor_LicenseNumber = @LicenseNumber";
        using var conn = _db.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Doctor>(query, new { LicenseNumber = licenseNumber });
    }

    public async Task<IEnumerable<Doctor>> GetBySpecialityIdAsync(int specialityId)
    {
        var query = "SELECT * FROM Doctor WHERE SpecialityId = @SpecialityId";
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<Doctor>(query, new { SpecialityId = specialityId });
    }

    public async Task<int> CreateAsync(Doctor doctor)
    {
        var query = @"
            INSERT INTO Doctor (
                Doctor_FirstName, Doctor_LastName, Doctor_Email, Doctor_Phone,
                Doctor_LicenseNumber, SpecialityId, Doctor_CreatedBy,Doctor_CreatedAt
            ) VALUES (
                @Doctor_FirstName, @Doctor_LastName, @Doctor_Email, @Doctor_Phone,
                @Doctor_LicenseNumber, @SpecialityId, @Doctor_CreatedBy, @Doctor_CreatedAt
            );
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        using var conn = _db.CreateConnection();
        return await conn.ExecuteScalarAsync<int>(query, doctor);
    }

    public async Task<bool> UpdateAsync(Doctor doctor)
    {
        var query = @"
            UPDATE Doctor SET
                Doctor_FirstName = @Doctor_FirstName,
                Doctor_LastName = @Doctor_LastName,
                Doctor_Email = @Doctor_Email,
                Doctor_Phone = @Doctor_Phone,
                Doctor_LicenseNumber = @Doctor_LicenseNumber,
                SpecialityId = @SpecialityId,
                Doctor_ModifiedBy = @Doctor_ModifiedBy,
                Doctor_ModifiedAt = SYSDATETIME()
            WHERE Doctor_Id = @Doctor_Id";

        using var conn = _db.CreateConnection();
        return await conn.ExecuteAsync(query, doctor) > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query = "DELETE FROM Doctor WHERE Doctor_Id = @Id";
        using var conn = _db.CreateConnection();
        return await conn.ExecuteAsync(query, new { Id = id }) > 0;
    }
}