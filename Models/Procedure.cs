namespace DogShelterMvc.Models
{
    public class Procedure
    {
        public int Id { get; set; }
        public string ProcName { get; set; } = string.Empty;
        public string DescrName { get; set; } = string.Empty;
        public int? ZdrZaznamid { get; set; }
        public MedicalRecord? Record { get; set; }
    }
}

