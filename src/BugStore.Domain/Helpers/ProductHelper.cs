using System.Text.RegularExpressions;

namespace BugStore.Domain.Helpers
{
    public static class ProductHelper
    {
        public static string GetSlug(string title)
        {
            Regex regex = new Regex("[ ]{1,}");
            return regex.Replace(title, "-").ToLower();
        }
    }
}
