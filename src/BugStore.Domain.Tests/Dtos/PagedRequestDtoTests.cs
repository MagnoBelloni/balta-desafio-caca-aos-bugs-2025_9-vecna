using BugStore.Domain.Constants;
using BugStore.Domain.Tests.TestHelpers;
using FluentAssertions;

namespace BugStore.Domain.Tests.Dtos
{
    public class PagedRequestDtoTests
    {
        [Fact]
        public void When_PagePropertiesDoesNotHaveValue_Should_ReturnDefaultValues()
        {
            var pagedRequest = new PagedRequestDtoHelper();

            var (page, pageSize) = pagedRequest.GetPageInfo();
            page.Should().Be(PageConstants.DEFAULT_PAGE);
            pageSize.Should().Be(PageConstants.DEFAULT_PAGE_SIZE);
        }

        [Fact]
        public void When_PagePropertiesHasValue_Should_ReturnValues()
        {
            var expectedPage = 2;
            var expectedPageSize = 20;

            var pagedRequest = new PagedRequestDtoHelper()
            {
                Page = expectedPage,
                PageSize = expectedPageSize
            };

            var (page, pageSize) = pagedRequest.GetPageInfo();
            page.Should().Be(expectedPage);
            pageSize.Should().Be(expectedPageSize);
        }
    }
}
