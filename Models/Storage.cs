namespace DogShelterMvc.Models
{
    public class Storage
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string Type { get; set; } = string.Empty;

        public override string ToString()
        {
            return Name;
        }
    }
}

