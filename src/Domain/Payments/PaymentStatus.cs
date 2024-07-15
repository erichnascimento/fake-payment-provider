namespace FakePaymentProvider.Domain.Payments;

public enum PaymentStatus
{
    Creating,
    Pending,
    Paid,
    Cancelled
}