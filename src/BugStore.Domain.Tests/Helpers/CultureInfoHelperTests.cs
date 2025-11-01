using BugStore.Domain.Helpers;
using FluentAssertions;

namespace BugStore.Domain.Tests.Helpers
{
    public class CultureInfoHelperTests
    {
        [Theory]
        [InlineData(1, "janeiro")]
        [InlineData(2, "fevereiro")]
        [InlineData(3, "março")]
        [InlineData(4, "abril")]
        [InlineData(5, "maio")]
        [InlineData(6, "junho")]
        [InlineData(7, "julho")]
        [InlineData(8, "agosto")]
        [InlineData(9, "setembro")]
        [InlineData(10, "outubro")]
        [InlineData(11, "novembro")]
        [InlineData(12, "dezembro")]
        public void GetMonthNameByInteger_Should_ReturnMonthNameAsString(int month, string expectedMonthName)
        {
            var monthName = CultureInfoHelper.GetMonthNameByInteger(month);
            monthName.Should().Be(expectedMonthName);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(13)]
        public void GetMonthNameByInteger_Should_ThrowException_WhenMonthIsInvalid(int month)
        {
            var result = Assert.Throws<Exception>(() => CultureInfoHelper.GetMonthNameByInteger(month));
            result.Message.Should().Be("Mês inválido");
        }
    }
}
