namespace GetInLineSchool.DTOs.Request
{
    public class CreateQueueRequest
    {
        public int? IDClass { get; set; }
        public string Subject { get; set; }
        public short Duration { get; set; }
    }
}
