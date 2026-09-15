namespace GetInLineSchool.Models
{
    public class Queue
    {
        public int IDQueue { get; set; }
        public short IDSchool { get; set; }
        public int? IDTeacher { get; set; }
        public int? IDStudent { get; set; }
        public int IDClass { get; set; }
        public string Subject { get; set; } = string.Empty;
        public short Duration { get; set; }
        public int Code { get; set; }
        public long Date { get; set; }
    }
}
