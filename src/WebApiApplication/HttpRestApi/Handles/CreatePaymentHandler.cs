using System.Text.Json.Serialization;
using FakePaymentProvider.Domain;
using FakePaymentProvider.Domain.Boundary.CreateBoleto;
using FakePaymentProvider.Library.Date;
using FakePaymentProvider.Library.Types;
using Microsoft.AspNetCore.Mvc;

namespace WebApiApplication.HttpRestApi.Handles;

public class CreatePaymentHandler(
    IServiceProvider serviceProvider
) : BaseHandler<CreatePaymentRequest, CreatePaymentResponse>
{
    protected override async Task<CreatePaymentResponse> DoHandle([FromBody] CreatePaymentRequest request)
    {
        if (request.IsBoleto is not true)
        {
            throw new NotImplementedException("Payment method not implemented");
        }
        
        var createBoletoRequest = new CreateBoletoRequest
        {
            Amount = new Money(
                amount: request.Amount,
                currency: new Currency(request.Currency)
            ),
            
            // TODO: Read from request
            
            CustomerName = "John Doe",
            CustomerEmail = "johndoe@example.com",
            DueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(3))
        };
        
        var useCase = new CreateBoletoUseCase(
            request: createBoletoRequest,
            timeService: serviceProvider.GetRequiredService<ITimeService>(),
            entityGateway: serviceProvider.GetRequiredService<IEntityGateway>()
        );
        
        var response = await useCase.Execute();

        return new CreatePaymentResponse
        {
            Id = response.BoletoId
        };
    }
}

public sealed record CreatePaymentRequest : BaseRequest
{
    public required string PaymentMethod;
    public required decimal Amount;
    public required string Currency;

    [JsonIgnore] public bool IsBoleto => PaymentMethod == "boleto";

    public override void Validate()
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

    public override string ToLoggableString()
    {
        return $"PaymentMethod={PaymentMethod}, Amount={Amount}, Currency={Currency}";
    }
}

public sealed record CreatePaymentResponse : CreatedResponse
{
    public required string Id;
    protected override string Uri => $"/v1/payments/{Id}";

    public override string ToLoggableString()
    {
        return $"Id={Id}";
    }
}