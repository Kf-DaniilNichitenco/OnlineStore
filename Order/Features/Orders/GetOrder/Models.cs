
using FastEndpoints;
using Order.Domain.Enums;

namespace Order.Features.Orders.GetOrder;

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
    public OrderDetail ResponseData { get; set; }
    public string Message => "";
}

public class OrderDetail
{
    public IEnumerable<ProductViewModel> Products { get; set; } = Enumerable.Empty<ProductViewModel>();

    public Guid ShippingDetailsId { get; set; }

    public string Address { get; set; } = string.Empty;

    public decimal Cost { get; set; }

    public OrderStatus Status { get; set; }

    public bool IsClosed { get; set; }
}