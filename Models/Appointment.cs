namespace GestionAtencionesAPI.Models;

public class Appointment
{
    public int Appointment_Id { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public DateTime Appointment_StartUtc { get; set; }
    public DateTime Appointment_EndUtc { get; set; }
    public string Appointment_Diagnosis { get; set; }
    public string Appointment_CreatedBy { get; set; }
    public DateTime Appointment_CreatedAt { get; set; }
    public string? Appointment_ModifiedBy { get; set; }
    public DateTime? Appointment_ModifiedAt { get; set; }
}
