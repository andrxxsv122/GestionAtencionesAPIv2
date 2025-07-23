namespace GestionAtencionesAPI.Models
{
    public class Patient
    {
        public int Patient_Id { get; set; }
        public string Patient_FirstName { get; set; }
        public string Patient_LastName { get; set; }
        public string Patient_RUT { get; set; }
        public DateTime Patient_DateOfBirth { get; set; }
        public string? Patient_Gender { get; set; }
        public string? Patient_Phone { get; set; }
        public string? Patient_Email { get; set; }
        public string? Patient_AddressLine1 { get; set; }
        public string? Patient_AddressLine2 { get; set; }
        public string? Patient_City { get; set; }
        public string? Patient_State { get; set; }
        public string Patient_PostalCode { get; set; }
        public string Patient_CreatedBy { get; set; }
        public DateTime Patient_CreatedAt { get; set; }
        public string? Patient_ModifiedBy { get; set; }
        public DateTime? Patient_ModifiedAt { get; set; }
    }
}
