namespace GetInLineSchool.Models
{
    public class Student
    {
        public int IDStudent { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int IDClass { get; set; }
        public int Role { get; set; }
        public bool IsActive { get; set; }
        public long RegistrationDate { get; set; }
    }
}
