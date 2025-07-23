namespace GestionAtencionesAPI.DTO
{
    public class AppointmentDTO
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int? Appointment_Id { get; set; }
        public DateTime Appointment_StartUtc { get; set; }
        public DateTime Appointment_EndUtc { get; set; }
        public string Appointment_Diagnosis { get; set; }
    }
}
