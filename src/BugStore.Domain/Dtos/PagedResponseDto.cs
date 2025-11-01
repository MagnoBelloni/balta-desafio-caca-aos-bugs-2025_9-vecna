namespace BugStore.Domain.Dtos
{
    public class PagedResponseDto<T>
    {
        public PagedResponseDto(T? response, int page, int pageSize, int totalCount)
        {
            Response = response;
            PageSize = pageSize;
            Page = page;
            TotalCount = totalCount;
        }

        public T? Response { get; set; }

        public int PageSize { get; set; }

        public int Page { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
