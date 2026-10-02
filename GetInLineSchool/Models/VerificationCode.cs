namespace GetInLineSchool.Models
{
    public class VerificationCode
    {
        public int IDCode { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public short Attempts { get; set; }
        public bool IsUsed { get; set; }
        public long ExpirationDate { get; set; }
    }
}
