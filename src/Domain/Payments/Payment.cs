using FakePaymentProvider.Library.Types;

namespace FakePaymentProvider.Domain.Payments;

public class Payment
{
    public readonly Id Id;
    public readonly Money Amount;
    public PaymentStatus Status { get; private set; }
    public readonly PaymentMethod Method;

    public readonly DateTime CreatedAt;
    public DateTime UpdatedAt { get; private set; }

    public static Payment Create(
        in PaymentMethod method,
        in Money amount,
        in DateTime now,
        in Id? id = null
    )
    {
        var payment = new Payment(
            id: id ?? Id.New(),
            amount: amount,
            status: PaymentStatus.Creating,
            method: method,
            createdAt: now,
            updatedAt: now
        );
        payment.Initialize(now: now);

        return payment;
    }

    private void Initialize(
        in DateTime now
    )
    {
        SetStatus(
            value: PaymentStatus.Pending,
            now: now
        );
    }

    private void SetStatus(PaymentStatus value, DateTime now)
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

    private Payment(
        Id id,
        Money amount,
        PaymentStatus status,
        PaymentMethod method,
        DateTime createdAt,
        DateTime updatedAt
    )
    {
        Id = id;
        Amount = amount;
        Status = status;
        Method = method;

        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
}