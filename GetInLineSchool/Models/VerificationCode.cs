namespace GetInLineSchool.Models
{
    public class VerificationCode
    {
        public int IDCode { get; set; }
        public int CodeValue { get; set; }
        public long ExpirationDate { get; set; }
    }
}
