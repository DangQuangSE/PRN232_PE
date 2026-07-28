namespace Q1.Models.Dtos
{
    public class CreateDirectorRequest
    {
        public string FullName { get; set; }
        public bool Male { get; set; }
        public DateOnly Dob { get; set; }
        public string Nationality { get; set; }
        public string Description { get; set; }
    }
}
