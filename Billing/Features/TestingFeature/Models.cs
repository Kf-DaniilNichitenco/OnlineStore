using FastEndpoints.Validation;

namespace Billing.Features.TestingFeature
{
    public class Request
    {

    }

    public class MyValidator : Validator<Request>
    {
        public MyValidator()
        {

        }
    }

    public class Response
    {
        public string Message => "This endpoint hasn't been implemented yet!";
    }
}