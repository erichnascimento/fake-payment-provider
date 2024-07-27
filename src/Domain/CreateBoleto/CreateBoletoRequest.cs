using FakePaymentProvider.Library.Types;

namespace FakePaymentProvider.Domain.CreateBoleto;

public sealed record CreateBoletoRequest
{
    public required string PaymentCode { get; init; }
    public required Money Amount { get; init; }
    public required DateOnly DueDate { get; init; }
    public string? PayerName { get; init; }
    public string? PayerEmail { get; init; }
    public string? PayerPhoneNumber { get; init; }
    public string? PayerDocument { get; init; }
    public Uri? PostbackUrl { get; init; }

    public bool HasPayerInfo =>
        PayerName is not null
        || PayerEmail is not null
        || PayerPhoneNumber is not null
        || PayerDocument is not null;
}