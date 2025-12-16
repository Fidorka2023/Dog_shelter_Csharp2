namespace DogShelterMvc.Models
{
    public class Dog
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string BodyColor { get; set; } = string.Empty;
        public DateTime DatumPrijeti { get; set; }
        public string DuvodPrijeti { get; set; } = string.Empty;
        public string StavPes { get; set; } = string.Empty;
        
        public int? UtulekId { get; set; }
        public Shelter? Utulek { get; set; }
        
        public int? KarantenaId { get; set; }
        public Quarantine? Karantena { get; set; }
        
        public int? MajitelId { get; set; }
        public Owner? Majitel { get; set; }
        
        public int? OtecId { get; set; }
        public Dog? Otec { get; set; }
        
        public int? MatkaId { get; set; }
        public Dog? Matka { get; set; }
        
        public int? ObrazekId { get; set; }
        public DogImage? DogImage { get; set; }

        public override string ToString()
        {
            return $"{Name} ({Age})";
        }
    }
}

