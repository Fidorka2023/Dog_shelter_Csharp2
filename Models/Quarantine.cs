namespace DogShelterMvc.Models
{
    public class Quarantine
    {
        public int Id { get; set; }
        public DateTime BeginOfDate { get; set; }
        public DateTime EndOfDate { get; set; }

        public override string ToString()
        {
            return $"{BeginOfDate} {(EndOfDate - BeginOfDate).TotalDays} days";
        }
    }
}

