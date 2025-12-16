namespace DogShelterMvc.Models
{
    public class DogImage
    {
        public int Id { get; set; }
        public byte[]? ImageData { get; set; }
        public string FileName { get; set; } = string.Empty;
    }
}

