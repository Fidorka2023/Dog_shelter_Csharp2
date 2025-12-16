namespace DogShelterMvc.Models
{
    public class DogHistory
    {
        public int Id { get; set; }
        public DateTime DateOfEvent { get; set; }
        public string EventDescription { get; set; } = string.Empty;
        public int? TypeId { get; set; }
        public int? DogId { get; set; }
        public Dog? Pes { get; set; }
        public string? Typ { get; set; }
    }
}

