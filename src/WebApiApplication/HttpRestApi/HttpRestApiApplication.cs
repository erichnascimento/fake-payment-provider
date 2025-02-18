using FakePaymentProvider.Domain;
using FakePaymentProvider.Domain.Payments;
using WebApiApplication.HttpRestApi.Handles.ConfirmPayment;
using WebApiApplication.HttpRestApi.Handles.CreatePayment;

namespace WebApiApplication.HttpRestApi;

public class HttpRestApiApplication
{
    private readonly WebApplication _webApplication;

    public void Run()
    {
        _webApplication.Run();
    }

    public static HttpRestApiApplication Create(string[] args)
    {
        return new HttpRestApiApplication(
            builder: WebApplication.CreateBuilder(args),
            configurator: new DefaultApplicationConfigurator()
        );
    }

    public HttpRestApiApplication(
        WebApplicationBuilder builder,
        IApplicationConfigurator configurator
    )
    {
        configurator.ConfigureHost(builder);
        _webApplication = builder.Build();
        ConfigureRoutes(_webApplication);
    }

    private static void ConfigureRoutes(WebApplication webApplication)
    {
        var v1RouteGroupBuilder = webApplication.MapGroup("/v1");
        var paymentsRouteGroupBuilder = v1RouteGroupBuilder.MapGroup("/payments");
        paymentsRouteGroupBuilder.MapPost("/",
            webApplication.Services.GetRequiredService<CreatePaymentHandler>().Handle);

        // TODO: extract this to a handler like CreatePaymentHandler.
        paymentsRouteGroupBuilder.MapGet("/{id}", async (string id) =>
        {
            // TODO: Use query segregation here.
            var entityGateway = webApplication.Services.GetRequiredService<IEntityGateway>();
            var payment = await entityGateway.GetPaymentById(id);
            if (payment is null)
            {
                return Results.NotFound();
            }

            var paymentBoleto = payment as PaymentBoleto;

            return Results.Ok(new GetPaymentResponse
            {
                Id = payment.Id,
                Status = payment.Status.ToString(),
                PaymentMethod = payment.Method.ToString(),
                CreatedAt = payment.CreatedAt,
                Amount = payment.Amount.Value,
                Currency = payment.Amount.Currency,
                PaidAmount = paymentBoleto?.PaidAmount?.Value,
                PaidOn = paymentBoleto?.PaidOn
            });
        });

        paymentsRouteGroupBuilder.MapPost("/{id}/confirm",
            webApplication.Services.GetRequiredService<ConfirmPaymentHandler>().Handle);

        // Echo route for testing purposes.
        v1RouteGroupBuilder.Map("/echo", async context =>
        {
            var request = context.Request;
            var requestBodyContent = await new StreamReader(request.Body).ReadToEndAsync();

            // TODO: respond with the request content

            Console.WriteLine($"Echo endpoint: request.Body={requestBodyContent}");
        });
    }
}

public sealed record GetPaymentResponse
{
    public required string Id;
    public required string Status;
    public required string PaymentMethod;
    public required DateTime CreatedAt;
    public required decimal Amount;
    public required string Currency;
    public decimal? PaidAmount;
    public DateOnly? PaidOn;
}