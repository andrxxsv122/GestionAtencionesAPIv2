using GestionAtencionesAPI.Models;

namespace GestionAtencionesAPI.Repositories
{
    public interface IAppointmentRepository
    {
        Task<IEnumerable<Appointment>> GetAllAsync();
        Task<Appointment?> GetByIdAsync(int id);
        Task<IEnumerable<Appointment>> GetByDateAsync(DateTime date);
        Task<IEnumerable<Appointment>> GetByDoctorAsync(int doctorId);
        Task<IEnumerable<Appointment>> GetByPatientAsync(int patientId);
        Task<IEnumerable<Appointment>> GetBySpecialityAsync(int specialityId);
        Task<double> GetDurationBySpecialityAsync(int specialityId);
        Task<bool> ExistsOverlapAsync(int doctorId, DateTime start, DateTime end);
        Task<bool> ExistAppointForPatientAsync(int patientId);
        Task<bool> ExistAppointForDoctorAsync(int doctorId);
        Task<int> CreateAsync(Appointment appointment);
        Task<bool> UpdateAsync(Appointment appointment);
        Task<bool> DeleteAsync(int id);
    }
}
