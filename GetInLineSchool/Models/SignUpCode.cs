namespace GetInLineSchool.Models
{
    public class SignUpCode
    {
        public int IDCode { get; set; }
        public int CodeValue { get; set; }
        public short IDSchool { get; set; }
        public int IDTeacher { get; set; }
        public byte CodeType { get; set; }
        public long ExpirationDate { get; set; }
    }
}
