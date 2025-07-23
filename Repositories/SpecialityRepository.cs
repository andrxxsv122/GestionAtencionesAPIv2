using Dapper;
using GestionAtencionesAPI.Data;
using GestionAtencionesAPI.Models;
using GestionAtencionesAPI.Repositories;

public class SpecialityRepository : ISpecialityRepository
{
    private readonly DbContext _db;

    public SpecialityRepository(DbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Speciality>> GetAllAsync()
    {
        var query = "SELECT * FROM Speciality";
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<Speciality>(query);
    }

    public async Task<Speciality?> GetByIdAsync(int id)
    {
        var query = "SELECT * FROM Speciality WHERE Speciality_Id = @Id";
        using var conn = _db.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Speciality>(query, new { Id = id });
    }
    public async Task<Speciality?> GetByNameAsync(string name)
    {
        var query = "SELECT * FROM Speciality WHERE Speciality_Name = @Name";
        using var conn = _db.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Speciality>(query, new { Name = name });
    }

    public async Task<int> CreateAsync(Speciality speciality)
    {
        var query = @"
            INSERT INTO Speciality (Speciality_Name, Speciality_Description, Speciality_CreatedBy, Speciality_CreatedAt)
            VALUES (@Speciality_Name, @Speciality_Description, @Speciality_CreatedBy, @Speciality_CreatedAt);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        using var conn = _db.CreateConnection();
        return await conn.ExecuteScalarAsync<int>(query, speciality);
    }

    public async Task<bool> UpdateAsync(Speciality speciality)
    {
        var query = @"
            UPDATE Speciality SET
                Speciality_Name = @Speciality_Name,
                Speciality_Description = @Speciality_Description,
                Speciality_ModifiedBy = @Speciality_ModifiedBy,
                Speciality_ModifiedAt = SYSDATETIME()
            WHERE Speciality_Id = @Speciality_Id";

        using var conn = _db.CreateConnection();
        return await conn.ExecuteAsync(query, speciality) > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query = "DELETE FROM Speciality WHERE Speciality_Id = @Id";
        using var conn = _db.CreateConnection();
        return await conn.ExecuteAsync(query, new { Id = id }) > 0;
    }
}