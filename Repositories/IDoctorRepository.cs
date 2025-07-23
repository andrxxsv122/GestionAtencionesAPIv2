using GestionAtencionesAPI.Models;

namespace GestionAtencionesAPI.Repositories
{
    public interface IDoctorRepository
    {
        public Task<IEnumerable<Doctor>> GetAllAsync();
        public Task<Doctor?> GetByIdAsync(int id);
        public Task<Doctor?> GetByLicenseAsync(string licenseNumber);
        public Task<IEnumerable<Doctor>> GetBySpecialityIdAsync(int specialityId);
        public Task<int> CreateAsync(Doctor doctor);
        public Task<bool> UpdateAsync(Doctor doctor);
        public Task<bool> DeleteAsync(int id);
    }
}
