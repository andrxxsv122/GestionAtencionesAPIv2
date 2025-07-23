using Dapper;
using GestionAtencionesAPI.Data;
using GestionAtencionesAPI.Models;
using GestionAtencionesAPI.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly DbContext _db;

    public PatientRepository(DbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Patient>> GetAllAsync()
    {
        var query = "SELECT * FROM Patient";
        using var connection = _db.CreateConnection();
        return await connection.QueryAsync<Patient>(query);
    }

    public async Task<Patient?> GetByIdAsync(int id)
    {
        var query = "SELECT * FROM Patient WHERE Patient_Id = @Id";
        using var connection = _db.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Patient>(query, new { Id = id });
    }

    public async Task<Patient?> GetByRutAsync(string rut)
    {
        var query = "SELECT * FROM Patient WHERE Patient_RUT = @Rut";
        using var connection = _db.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Patient>(query, new { Rut = rut });
    }

    public async Task<int> CreateAsync(Patient patient)
    {
        var query = @"
        INSERT INTO Patient (
            Patient_FirstName, Patient_LastName, Patient_RUT, Patient_DateOfBirth, Patient_Gender,
            Patient_Phone, Patient_Email, Patient_AddressLine1, Patient_AddressLine2,
            Patient_City, Patient_State, Patient_PostalCode, Patient_CreatedBy
        ) VALUES (
            @Patient_FirstName, @Patient_LastName, @Patient_RUT, @Patient_DateOfBirth, @Patient_Gender,
            @Patient_Phone, @Patient_Email, @Patient_AddressLine1, @Patient_AddressLine2,
            @Patient_City, @Patient_State, @Patient_PostalCode, @Patient_CreatedBy
        );

        SELECT CAST(SCOPE_IDENTITY() AS INT);
    ";

        using var connection = _db.CreateConnection();
        var id = await connection.ExecuteScalarAsync<int>(query, patient);
        return id;
    }

    public async Task<bool> UpdateAsync(Patient patient)
    {
        var query = @"
            UPDATE Patient SET
                Patient_FirstName = @Patient_FirstName,
                Patient_LastName = @Patient_LastName,
                Patient_DateOfBirth = @Patient_DateOfBirth,
                Patient_Gender = @Patient_Gender,
                Patient_Phone = @Patient_Phone,
                Patient_Email = @Patient_Email,
                Patient_AddressLine1 = @Patient_AddressLine1,
                Patient_AddressLine2 = @Patient_AddressLine2,
                Patient_City = @Patient_City,
                Patient_State = @Patient_State,
                Patient_PostalCode = @Patient_PostalCode,
                Patient_ModifiedBy = @Patient_ModifiedBy,
                Patient_ModifiedAt = SYSDATETIME()
            WHERE Patient_Id = @Patient_Id";

        using var connection = _db.CreateConnection();
        var rows = await connection.ExecuteAsync(query, patient);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query = "DELETE FROM Patient WHERE Patient_Id = @Id";
        using var connection = _db.CreateConnection();
        var rows = await connection.ExecuteAsync(query, new { Id = id });
        return rows > 0;
    }
}