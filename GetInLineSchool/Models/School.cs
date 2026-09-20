namespace GetInLineSchool.Models
{
    public class School
    {
        public short IDSchool { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string UnifiedIdentificationCode { get; set; }
        public string MateriallyResponsiblePerson { get; set; }
        public int SubscriptionPlan { get; set; }
        public bool IsPaid { get; set; }
        public long LastPaymentDate { get; set; }
        public bool EnableStudents { get; set; }
        public int SchoolCode { get; set; }
        public long RegistrationDate { get; set; }
    }
}
