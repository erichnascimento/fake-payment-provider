using System.Text.Json;
using System.Text.Json.Serialization;
using FakePaymentProvider.Domain;
using FakePaymentProvider.Domain.Boundary.CreateBoleto;
using FakePaymentProvider.Infra.EntityGateway.Memory;
using FakePaymentProvider.Library.Date;
using FakePaymentProvider.Library.Types;
using Microsoft.AspNetCore.Mvc;

namespace WebApiApplication;

public class HttpRestApiApplication
{
    private readonly WebApplication _webApplication;

    public void Run()
    {
        _webApplication.Run();
    }

    internal HttpRestApiApplication(string[] args)
    {
        // TODO: Extract the setup

        var builder = WebApplication.CreateBuilder(args);
        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);

            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.IncludeFields = true;
        });
        builder.Services.AddSingleton<ITimeService, SystemTimeService>();
        builder.Services.AddSingleton<IEntityGateway, InMemoryEntityGateway>();

        var webApplication = builder.Build();

        var v1RouteGroupBuilder = webApplication.MapGroup("/v1");
        var paymentsRouteGroupBuilder = v1RouteGroupBuilder.MapGroup("/payments");
        paymentsRouteGroupBuilder.MapPost("/", async ([FromBody] PostPaymentRequest request) =>
        {
            try
            {
                request.Validate();
            }
            catch (ArgumentException e)
            {
                return Results.BadRequest(e.Message);
            }

            if (request.IsBoleto)
            {
                var createBoletoRequest = new CreateBoletoRequest
                {
                    Amount = new Money(
                        amount: request.Amount,
                        currency: new Currency(request.Currency)
                    ),
                    CustomerName = "John Doe",
                    CustomerEmail = "johndoe@example.com",
                    DueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(3))
                };
                var useCase = new CreateBoletoUseCase(
                    request: createBoletoRequest,
                    timeService: webApplication.Services.GetRequiredService<ITimeService>(),
                    entityGateway: webApplication.Services.GetRequiredService<IEntityGateway>()
                );
                var response = await useCase.Execute();

                return Results.Created(
                    uri: $"/v1/payments/{response.BoletoId}",
                    value: new PostPaymentResponse
                    {
                        Id = response.BoletoId
                    });
            }

            throw new NotImplementedException("Payment method not implemented");
        });

        paymentsRouteGroupBuilder.MapGet("/{id}", (string id) =>
        {
            // TODO: Use query segregation here.
            var entityGateway = webApplication.Services.GetRequiredService<IEntityGateway>();
            var payment = entityGateway.GetPaymentById(id);
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
                Amount = payment.Amount.Amount,
                Currency = payment.Amount.Currency
            });
        });

        _webApplication = webApplication;
    }
}

public sealed record PostPaymentRequest
{
    public required string PaymentMethod;
    public required decimal Amount;
    public required string Currency;

    [JsonIgnore] public bool IsBoleto => PaymentMethod == "boleto";

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(PaymentMethod))
        {
            throw new ArgumentException("PaymentMethod is required");
        }

        switch (PaymentMethod)
        {
            case "boleto":
                break;
            default:
                throw new ArgumentException("PaymentMethod is invalid");
        }

        if (Amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than 0");
        }

        if (string.IsNullOrWhiteSpace(Currency))
        {
            throw new ArgumentException("Currency is required");
        }
    }
}

public sealed record PostPaymentResponse
{
    public required string Id;
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

[JsonSerializable(typeof(PostPaymentRequest))]
[JsonSerializable(typeof(PostPaymentResponse))]
[JsonSerializable(typeof(GetPaymentResponse))]
internal partial class AppJsonSerializerContext : JsonSerializerContext;