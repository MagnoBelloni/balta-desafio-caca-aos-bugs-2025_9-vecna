using BugStore.Domain.Constants;
using BugStore.Domain.Exceptions;
using System.Globalization;

namespace BugStore.Domain.Helpers
{
    public static class CultureInfoHelper
    {
        public static string GetMonthNameByInteger(int month, string culture = CultureConstants.BR_CULTURE_INFO)
        {
            if (month is < 1 or > 12)
                throw new CustomAppException("Mês inválido");

            var cultureInfo = new CultureInfo(culture);
            return cultureInfo.DateTimeFormat.GetMonthName(month);
        }
    }
}
