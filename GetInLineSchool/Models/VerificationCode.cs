namespace GetInLineSchool.Models
{
    public class VerificationCode
    {
        public int IDCode { get; set; }
        public int CodeValue { get; set; }
        public short Attempts { get; set; }
        public bool IsUsed { get; set; }
        public long ExpirationDate { get; set; }
    }
}
