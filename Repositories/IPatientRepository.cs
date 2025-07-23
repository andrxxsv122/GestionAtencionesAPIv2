using GestionAtencionesAPI.Models;

namespace GestionAtencionesAPI.Repositories
{
    public interface IPatientRepository
    {
        public Task<IEnumerable<Patient>> GetAllAsync();
        public Task<Patient?> GetByIdAsync(int id);
        public Task<Patient?> GetByRutAsync(string rut);
        public Task<int> CreateAsync(Patient patient);
        public Task<bool> UpdateAsync(Patient patient);
        public Task<bool> DeleteAsync(int id);
    }
}