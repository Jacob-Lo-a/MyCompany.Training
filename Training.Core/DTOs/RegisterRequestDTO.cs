namespace Training.Core.DTOs
{
    public class RegisterRequestDTO
    {
        public string? Username {  get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public int Age { get; set; }
    }
}
