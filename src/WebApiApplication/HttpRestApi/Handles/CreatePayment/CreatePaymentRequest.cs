using System.Text.Json.Serialization;
using WebApiApplication.HttpRestApi.Handles.Base;

namespace WebApiApplication.HttpRestApi.Handles.CreatePayment;

public sealed record CreatePaymentRequest : BaseRequest
{
    public required decimal Amount;
    public required string Currency;
    public required string PaymentMethod;
    public required DateOnly DueDate;

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