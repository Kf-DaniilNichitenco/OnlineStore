
namespace Order.Features.Orders.GetOrder;

using FastEndpoints.Validation;
using Domain.Entities;

public class GetOrderDetailsRequest
{
    public Guid Id { get; set; }
}

public class Validator : Validator<GetOrderDetailsRequest>
{
    public Validator()
    {

    }
}

public class GetOrderDetailsResponse
{
    public Order ResponseData { get; set; }
    public string Message => "";
}