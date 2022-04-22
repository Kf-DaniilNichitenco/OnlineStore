namespace Order.Features.Orders.SearchOrder;

using Domain.Entities;
using FastEndpoints.Validation;
using Domain.Enums;
using Views;
using Views.Queries;
public class SearchOrderQuery : SearchQuery
{

}

public class Validator : Validator<SearchOrderQuery>
{
    public Validator()
    {
        RuleFor(x => x.Value).NotEmpty().WithMessage("Value is empty");
    }
}

public class SearchOrderResultResponse
{
    public string Message => "";
    public PageableResult<SearchOrderItem>? ResponseData { get; set; }
}

public class SearchOrderItem
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public string Address { get; set; } = string.Empty;
    public decimal Cost {get;set;}
    public OrderStatus Status {get;set;}
    public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();

}