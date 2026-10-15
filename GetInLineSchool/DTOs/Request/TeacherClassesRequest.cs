namespace GetInLineSchool.DTOs.Request
{
    public class TeacherClass
    {
        public int IDClass { get; set; }
        public string Subject { get; set; }
    }
    public class TeacherClassesRequest
    {
        public List<TeacherClass> TeacherClasses { get; set; }
    }
}
