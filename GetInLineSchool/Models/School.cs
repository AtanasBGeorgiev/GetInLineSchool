namespace GetInLineSchool.Models
{
    public class School
    {
        public short IDSchool { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string UnifiedIdentificationCode { get; set; } = string.Empty;
        public string MateriallyResponsiblePerson { get; set; } = string.Empty;
        public int SubscriptionPlan { get; set; }
        public bool IsPaid { get; set; }
        public long LastPaymentDate { get; set; }
        public bool EnableStudents { get; set; }
        public int SchoolCode { get; set; }
        public long RegistrationDate { get; set; }
    }
}
