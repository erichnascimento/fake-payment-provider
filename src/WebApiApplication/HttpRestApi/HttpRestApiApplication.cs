using System.Text.Json;
using FakePaymentProvider.Domain;
using FakePaymentProvider.Domain.Boundary.CreateBoleto;
using FakePaymentProvider.Infra.EntityGateway.Memory;
using FakePaymentProvider.Library.Date;
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
        return new HttpRestApiApplication(args);
    }

    private HttpRestApiApplication(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder);

        var webApplication = builder.Build();
        ConfigureRoutes(webApplication);

        _webApplication = webApplication;
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);

            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.IncludeFields = true;
        });

        builder.Services.AddSingleton<ITimeService, SystemTimeService>();
        builder.Services.AddSingleton<IEntityGateway, InMemoryEntityGateway>();
        builder.Services.AddSingleton<ICreateBoletoUseCase, CreateBoletoUseCase>();
        builder.Services.AddSingleton<CreatePaymentHandler>();
    }

    private static void ConfigureRoutes(WebApplication webApplication)
    {
        var v1RouteGroupBuilder = webApplication.MapGroup("/v1");
        var paymentsRouteGroupBuilder = v1RouteGroupBuilder.MapGroup("/payments");
        paymentsRouteGroupBuilder.MapPost("/",
            webApplication.Services.GetRequiredService<CreatePaymentHandler>().Handle);

        paymentsRouteGroupBuilder.MapGet("/{id}", async (string id) =>
        {
            // TODO: Use query segregation here.
            var entityGateway = webApplication.Services.GetRequiredService<IEntityGateway>();
            var payment = await entityGateway.GetPaymentById(id);
            if (payment is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(new GetPaymentResponse
            {
                Id = payment.Id,
                Status = payment.Status.ToString(),
                PaymentMethod = payment.Method.ToString(),
                CreatedAt = payment.CreatedAt,
                Amount = payment.Amount.Value,
                Currency = payment.Amount.Currency
            });
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
}