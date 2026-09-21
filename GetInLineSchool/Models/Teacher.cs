namespace GetInLineSchool.Models
{
    public class Teacher
    {
        public int IDTeacher { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public short IDSchool { get; set; }
        public int Role { get; set; }
        public bool IsActive { get; set; }
        public long RegistrationDate { get; set; }
    }
}
