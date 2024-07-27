using FakePaymentProvider.Library.Types;

namespace FakePaymentProvider.Domain.ConfirmPayment;

public sealed record ConfirmPaymentUseCaseRequest
{
    public required Id PaymentId { get; init; }
    public required Money PaidAmount { get; init; }
    public DateOnly? PaidOn { get; init; }
}