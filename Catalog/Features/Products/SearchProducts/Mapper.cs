using Catalog.Domain.Entities;
using Catalog.Views;
using FastEndpoints;

namespace Catalog.Features.Products.SearchProducts
{
    public class Mapper : Mapper<SearchProductQuery, SearchProductResultResponse, IEnumerable<Product>>
    {
        public override SearchProductResultResponse FromEntity(IEnumerable<Product> products)
        {
            var productsList = products.ToList();

            var data = new List<SearchProductItem>();

            foreach (var product in productsList)
            {
                var item = new SearchProductItem
                {
                    Id = product.Id,
                    ShortName = product.ShortName ?? product.Name,
                    ShortDescription = product.ShortDescription ?? product.Description,
                };

                data.Add(item);
            }

            var result = new SearchProductResultResponse
            {
                ResponseData = new PageableResult<SearchProductItem>
                {
                    Data = data,
                    Total = productsList.Count,
                    Size = data.Count,
                    Page = 1
                }
            };

            return result;
        }
    }
}