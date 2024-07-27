using WebApiApplication.HttpRestApi.Handles.Base;
using WebApiApplication.HttpRestApi.Handles.CreatePayment.Types;

namespace WebApiApplication.HttpRestApi.Handles.CreatePayment;

public sealed record CreatePaymentRequest : BaseRequest
{
    public required decimal Amount;
    public required string Currency;
    public required string PaymentMethod;
    public required string PaymentCode;
    public DateOnly? DueDate;
    public bool? WithPix;
    public Payer? Payer;
    public string? PostbackUrl;

    public override void Validate()
    {
        // TODO: extract this validation to a separate class
        
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

        if (string.IsNullOrWhiteSpace(PaymentCode))
        {
            throw new ArgumentException("PaymentCode is required");
        }

        if (Amount < 0)
        {
            throw new ArgumentException("Amount cannot be negative");
        }

        if (string.IsNullOrWhiteSpace(Currency))
        {
            throw new ArgumentException("Currency is required");
        }
        
        if (PostbackUrl is not null && !Uri.TryCreate(PostbackUrl, UriKind.Absolute, out _))
        {
            throw new ArgumentException("PostbackUrl is invalid");
        }
        
        // Boleto
        if (PaymentMethod == "boleto")
        {
            if (DueDate is null)
            {
                throw new ArgumentException("DueDate is required for boleto");
            }

            if (Payer is null)
            {
                throw new ArgumentException("Payer is required for boleto");
            }

            if (Payer.Document is null)
            {
                throw new ArgumentException("Payer.Document is required for boleto");
            }

            if (string.IsNullOrWhiteSpace(Payer.Document.Type))
            {
                throw new ArgumentException("Payer.Document.Type is required for boleto");
            }
            
            if (Payer.Document.Type != "CPF" && Payer.Document.Type != "CNPJ")
            {
                throw new ArgumentException("Payer.Document.Type is invalid for boleto");
            }

            if (string.IsNullOrWhiteSpace(Payer.Document.Number))
            {
                throw new ArgumentException("Payer.Document.Number is required for boleto");
            }
            
            // TODO: Validate the document based on its type
        }
    }

    public override string ToLoggableString()
    {
        return $"PaymentMethod={PaymentMethod}, Amount={Amount}, Currency={Currency}, DueDate={DueDate}, WithPix={WithPix}, Payer={Payer}";
    }
}