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

            var data = productsList.Select(product => new SearchProductItem
            {
                Id = product.Id, 
                ShortName = product.ShortName ?? product.Name, 
                ShortDescription = product.ShortDescription ?? product.Description, 
                Tags = product.Tags.Select(x => new TagViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
            })
                .ToList();

            var result = new SearchProductResultResponse
            {
                ResponseData = new PageableResult<SearchProductItem>
                {
                    Data = data,
                    Total = productsList.Count,
                    Size = data.Count,
                    Page = 1
                },
            };

            return result;
        }
    }
}