using System.Text;
using System.Text.RegularExpressions;

namespace tiki_shop.Models.Common
{
    public class Common
    {
        public static string ConvertToUnSign(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            string unsign = regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D').ToLower();
            string slug = unsign.Replace(' ', '-');
            return slug;
        }
    }
}
