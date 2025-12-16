namespace DogShelterMvc.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Psc { get; set; } = string.Empty;
        public int Number { get; set; }

        public override string ToString()
        {
            return $"{Street} {Number} {City} {Psc}";
        }
    }
}

