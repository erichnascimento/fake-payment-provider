using FakePaymentProvider.Library.Types;

namespace FakePaymentProvider.Domain.CreateBoleto;

public sealed record CreateBoletoResponse
{
    public Id BoletoId { get; init; }
}