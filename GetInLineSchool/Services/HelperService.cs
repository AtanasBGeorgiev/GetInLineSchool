namespace GetInLineSchool.Services
{
    public class HelperService
    {
        public static List<int> SchoolCodes = new List<int>();
        public static int CreateSchoolCode()
        {
            Random rnd = new Random();
            return rnd.Next(000, 999);
        }
    }
}
