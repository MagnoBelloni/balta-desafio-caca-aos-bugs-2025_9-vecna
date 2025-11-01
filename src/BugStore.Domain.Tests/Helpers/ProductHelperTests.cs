using BugStore.Domain.Helpers;
using FluentAssertions;

namespace BugStore.Domain.Tests.Helpers
{
    public class ProductHelperTests
    {
        [Theory]
        [InlineData("Titulo Bacana     aqui", "titulo-bacana-aqui")]
        [InlineData("titulo BACANA aqui", "titulo-bacana-aqui")]
        public void GetSlug_Should_ReturnTitleWithSpacesReplacedWithHifenAndToLower(string title, string expectedResult)
        {
            var result = ProductHelper.GetSlug(title);
            result.Should().Be(expectedResult);
        }
    }
}
