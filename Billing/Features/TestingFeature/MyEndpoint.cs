using FastEndpoints;

namespace Billing.Features.TestingFeature;

public class MyEndpoint : Endpoint<Request, Response, MyMapper>
{
    public override void Configure()
    {
        Post("/route/path/here");
    }

    public override Task HandleAsync(Request r, CancellationToken c)
    {
        return SendAsync(Response);
    }
}