using BugStore.Application.Products.Responses;
using BugStore.Domain.Dtos;
using MediatR;

namespace BugStore.Application.Products.Requests;

public class GetProductRequest : PagedRequestDto, IRequest<PagedResponseDto<GetProductResponse>>
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Slug { get; set; }
    public decimal? Price { get; set; }
}