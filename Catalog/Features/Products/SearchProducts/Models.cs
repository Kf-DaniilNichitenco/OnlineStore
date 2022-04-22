using Catalog.Domain.Entities;
using Catalog.Views;
using Catalog.Views.Queries;
using FastEndpoints;

namespace Catalog.Features.Products.SearchProducts
{
    public class SearchProductQuery: SearchQuery
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public class Validator : Validator<SearchProductQuery>
    {
        public Validator()
        {

        }
    }

    public class SearchProductResultResponse
    {
        public string Message => "";
        public PageableResult<SearchProductItem>? ResponseData { get; set; }
    }

    public class SearchProductItem
    {
        public Guid Id { get; set; }

        public string ShortName { get; set; } = string.Empty;

        public string ShortDescription { get; set; } = string.Empty;

        public IEnumerable<Tag> Tags { get; set; } = Enumerable.Empty<Tag>();
    }
}