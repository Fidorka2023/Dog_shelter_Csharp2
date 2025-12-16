namespace DogShelterMvc.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Uname { get; set; } = string.Empty;
        public string Hash { get; set; } = string.Empty;
        public ulong Perms { get; set; }
    }
}

