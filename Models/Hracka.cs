namespace DogShelterMvc.Models
{
    public class Hracka
    {
        public int Id { get; set; }
        public string Nazev { get; set; } = string.Empty;
        public int Pocet { get; set; }
        public int? SkladID { get; set; }
        public Storage? Sklad { get; set; }
    }
}

