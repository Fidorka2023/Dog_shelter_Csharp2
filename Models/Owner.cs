namespace DogShelterMvc.Models
{
    public class Owner
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Email { get; set; }
        
        public int? AddressID { get; set; }
        public Address? Adresa { get; set; }
    }
}

