using FakePaymentProvider.Library.Types;

namespace FakePaymentProvider.Domain.ConfirmPayment;

public sealed record ConfirmPaymentUseCaseResponse
{
    public required Id PaymentId { get; init; }
    public required string PaymentCode { get; init; }
    public required string Status { get; init; }
    public required Money PaidAmount { get; init; }
    public required DateOnly PaidOn { get; init; }
}