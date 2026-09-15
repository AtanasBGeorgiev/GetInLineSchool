namespace GetInLineSchool.Models
{
    public class Teacher
    {
        public int IDTeacher { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public short IDSchool { get; set; }
        public int Role { get; set; }
        public bool IsActive { get; set; }
        public long RegistrationDate { get; set; }
    }
}
