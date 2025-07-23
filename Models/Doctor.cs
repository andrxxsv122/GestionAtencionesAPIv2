namespace GestionAtencionesAPI.Models
{
    public class Doctor
    {
        public int Doctor_Id { get; set; }
        public string Doctor_FirstName { get; set; }
        public string Doctor_LastName { get; set; }
        public string? Doctor_Email { get; set; }
        public string? Doctor_Phone { get; set; }
        public string Doctor_LicenseNumber { get; set; }
        public int SpecialityId { get; set; }
        public string Doctor_CreatedBy { get; set; }
        public DateTime Doctor_CreatedAt { get; set; }
        public string? Doctor_ModifiedBy { get; set; }
        public DateTime? Doctor_ModifiedAt { get; set; }
    }
}
