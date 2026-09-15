namespace GetInLineSchool.Models
{
    public class Admin
    {
        public byte IDAdmin { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int Role { get; set; }
        public long RegistrationDate { get; set; }
    }
}
