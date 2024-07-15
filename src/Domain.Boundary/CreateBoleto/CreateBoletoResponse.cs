using FakePaymentProvider.Library.Types;

namespace FakePaymentProvider.Domain.Boundary.CreateBoleto;

public sealed record CreateBoletoResponse
{
    public Id BoletoId { get; init; }
}