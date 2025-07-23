using GestionAtencionesAPI.Models;

namespace GestionAtencionesAPI.Repositories
{
    public interface ISpecialityRepository
    {
        public Task<IEnumerable<Speciality>> GetAllAsync();
        public Task<Speciality?> GetByIdAsync(int id);
        public Task<Speciality?> GetByNameAsync(string name);
        public Task<int> CreateAsync(Speciality speciality);
        public Task<bool> UpdateAsync(Speciality speciality);
        public Task<bool> DeleteAsync(int id);
    }
}
