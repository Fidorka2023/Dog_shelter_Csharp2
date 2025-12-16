namespace DogShelterMvc.Models
{
    public class Log
    {
        public int Id { get; set; }
        public string CUser { get; set; } = string.Empty;
        public DateTime EventTime { get; set; }
        public string TableName { get; set; } = string.Empty;
        public string Operation { get; set; } = string.Empty;
        public string OldValue { get; set; } = string.Empty;
        public string NewValue { get; set; } = string.Empty;
    }
}

