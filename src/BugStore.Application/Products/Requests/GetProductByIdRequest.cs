using BugStore.Application.Products.Responses;
using MediatR;

namespace BugStore.Application.Products.Requests;

public class GetProductByIdRequest : IRequest<GetProductByIdResponse?>
{
    public Guid Id { get; set; }
}