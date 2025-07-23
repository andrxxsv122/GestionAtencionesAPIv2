namespace GestionAtencionesAPI.Models
{
    public class Speciality
    {
        public int Speciality_Id { get; set; }
        public string Speciality_Name { get; set; }
        public string? Speciality_Description { get; set; }
        public string Speciality_CreatedBy { get; set; }
        public DateTime Speciality_CreatedAt { get; set; }
        public string? Speciality_ModifiedBy { get; set; }
        public DateTime? Speciality_ModifiedAt { get; set; }
    }
}
