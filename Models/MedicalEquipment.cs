namespace DogShelterMvc.Models
{
    public class MedicalEquipment
    {
        public int Id { get; set; }
        public string MedicalName { get; set; } = string.Empty;
        public int Count { get; set; }
        public int? SkladID { get; set; }
        public Storage? Sklad { get; set; }
    }
}

