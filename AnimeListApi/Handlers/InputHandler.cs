using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using static System.Text.RegularExpressions.Regex;

namespace AnimeListApi.Handlers
{
    public static class InputHandler
    {
        public static string SanitizeInput(string input)
        {
            var cleanedInput = Regex.Replace(input, "<.*?>", string.Empty);
            cleanedInput = cleanedInput.Trim();
            cleanedInput = HtmlEncoder.Default.Encode(cleanedInput);
            return cleanedInput;
        }
    }
}