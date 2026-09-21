namespace GetInLineSchool.Models
{
    public class Admin
    {
        public byte IDAdmin { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Role { get; set; }
        public long RegistrationDate { get; set; }
    }
}
