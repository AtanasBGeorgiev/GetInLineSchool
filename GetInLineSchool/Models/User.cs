namespace GetInLineSchool.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int Role { get; set; }
        public bool IsActive { get; set; }
        public long RegistrationDate { get; set; }
    }
}
