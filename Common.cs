using System.Web;

namespace LeaveApplication
{
    public static class Common
    {
        public static string ReplaceDotsWithDesh(string email)
        {
            email = email.Replace(".", "-");
            return email;
        }

        public static string ReplaceDeshWithDots(string email)
        {
            email = email.Replace("-", ".");
            return email;
        }
    }
}
