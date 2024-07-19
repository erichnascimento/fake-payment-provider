using FakePaymentProvider.Library.Types;

namespace FakePaymentProvider.Domain.Payments;

public abstract class Payment(
    Id id,
    Money amount,
    PaymentStatus status,
    PaymentMethod method,
    Payer? payer,
    DateTime createdAt,
    DateTime updatedAt)
{
    public readonly Id Id = id;
    public readonly Money Amount = amount;
    public PaymentStatus Status { get; private set; } = status;
    public readonly PaymentMethod Method = method;
    public readonly Payer? Payer = payer;

    public readonly DateTime CreatedAt = createdAt;
    public DateTime UpdatedAt { get; private set; } = updatedAt;

    protected void SetStatus(PaymentStatus value, DateTime now)
    {
        PaymentStatusTransitionValidator.CheckIsValidTransition(
            from: Status,
            to: value
        );

        PaymentStatusTransitionValidator.CheckPreConditions(
            payment: this,
            to: value
        );

        Status = value;
        AfterUpdate(now);
    }

    private void AfterUpdate(DateTime now)
    {
        UpdatedAt = now;
    }
}