using FastEndpoints;

namespace Catalog.Features.Products.Test
{
    public class MyEndpoint : Endpoint<Request, Response, MyMapper>
    {
        public override void Configure()
        {
            Get("/tea/products/test");
            Description(x =>
            {
                x.WithName("Test");
            });
            AllowAnonymous();
        }

        public override Task HandleAsync(Request r, CancellationToken c)
        {
            var result = new Response();
            return SendAsync(result);
        }
    }
}