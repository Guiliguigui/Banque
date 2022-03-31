using System.Text.RegularExpressions;

namespace Banque.Helpers
{
    public class Tools
    {
        public static string ClearMultipleSpace(string chaine)
        {
            string pattern = @"\s+";
            string CleanedString = Regex.Replace(chaine, pattern, " ");

            return CleanedString;
        }

        public static string FormatPhone(string phone)
        {
            string pattern = @"[\.\-\s]+";
            string FormatedString = Regex.Replace(phone, pattern, "");

            pattern = @"^(0|33)";
            FormatedString = Regex.Replace(FormatedString, pattern, "+33");

            return ClearMultipleSpace(FormatedString);
        }
    }
}
