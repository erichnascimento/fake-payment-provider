using FakePaymentProvider.Library.Types;

namespace FakePaymentProvider.Domain.Boundary.CreateBoleto;

public sealed record CreateBoletoRequest
{
    public  string CustomerName { get; init; }
    public string CustomerEmail { get; init; }
    public Money Amount { get; init; }
    public DateOnly DueDate { get; init; }
}