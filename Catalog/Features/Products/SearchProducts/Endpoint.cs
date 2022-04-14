using FastEndpoints;

namespace Catalog.Features.Products.SearchProducts
{
    public class Endpoint : Endpoint<SearchProductQuery, SearchProductResultResponse, Mapper>
    {
        public override void Configure()
        {
            Get("/catalog/products/search/{id}/{name}");
            Description(x =>
            {
                x.WithName("SearchProducts");
            });
            
            AllowAnonymous();
        }

        public override Task HandleAsync(SearchProductQuery searchProductQuery, CancellationToken cancellationToken)
        {
            var products = Data.Products;

            var result = Map.FromEntity(products);

            return SendAsync(result, cancellation: cancellationToken);
        }
    }
}