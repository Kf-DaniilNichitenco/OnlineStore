namespace Shipping.Features.Shipping.GetShippingDetails;
using Domain.Entities;

public static class Data
{
    public static IQueryable<Shipping> Shipping => new List<Shipping>().AsQueryable();
}