namespace Order.Features.Orders.GetOrder;

using FastEndpoints;
using Domain.Entities;

public class Mapper : Mapper<GetOrderDetailsRequest, GetOrderDetailsResponse, Order>
{
    public override GetOrderDetailsResponse FromEntity(Order order)
    {
        var data = new OrderDetail
        {
            Products = order.Products.Select(x => new ProductViewModel
            {
                Id = x.ExternalId, Tags = x.Tags, Amount = x.Amount, Name = x.Name
            }),
            Address = order.Address,
            Cost = order.Cost,
            Status = order.Status,
            IsClosed = order.IsClosed,
            ShippingDetailsId = order.ShippingDetailsId
        };

        var result = new GetOrderDetailsResponse
        {
            ResponseData = data
        };

        return result;
    }
}