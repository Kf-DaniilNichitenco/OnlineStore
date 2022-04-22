namespace Order.Features.Orders.SearchOrder;

using Views;
using FastEndpoints;
using Domain.Entities;

public class Mapper : Mapper<SearchOrderQuery, SearchOrderResultResponse, IEnumerable<Order>>
{
    public override SearchOrderResultResponse FromEntity(IEnumerable<Order> orders)
    {
        var ordersList = orders.ToList();

        var data = ordersList.Select(order => new SearchOrderItem
            {
                Id = order.Id,
                OwnerId = order.OwnerId,
                Cost = order.Cost,
                Address = order.Address,
                Status = order.Status,
                Products = order.Products,
            })
            .ToList();

        var result = new SearchOrderResultResponse
        {
            ResponseData = new PageableResult<SearchOrderItem>
            {
                Data = data,
                Total = ordersList.Count,
                Size = data.Count,
                Page = 1
            },
        };

        return result;
    }
}