namespace DogShelterMvc.Models
{
    public class Shelter
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string? Email { get; set; }
        
        public int? AddressID { get; set; }
        public Address? Adresa { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}

