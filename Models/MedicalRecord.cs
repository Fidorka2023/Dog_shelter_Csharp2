namespace DogShelterMvc.Models
{
    public class MedicalRecord
    {
        public int Id { get; set; }
        public DateTime DateRec { get; set; }
        public int? TypeProcId { get; set; }
        public int? DogId { get; set; }
        public Dog? Dog { get; set; }
        public Procedure? Type { get; set; }
    }
}

