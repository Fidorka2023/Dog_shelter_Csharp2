namespace DogShelterMvc.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime DateOfReceipt { get; set; }
        public DateTime DateOfTransfer { get; set; }
        public int? DogId { get; set; }
        public Dog? Pes { get; set; }
    }
}

