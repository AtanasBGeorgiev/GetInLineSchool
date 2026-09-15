namespace GetInLineSchool.Models
{
    public class TeacherClass
    {
        public int IDTeacher { get; set; }
        public int IDClass { get; set; }
        public string Subject { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
