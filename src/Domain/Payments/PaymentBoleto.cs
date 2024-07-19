using FakePaymentProvider.Library.Types;

namespace FakePaymentProvider.Domain.Payments;

public sealed class PaymentBoleto : Payment
{
    public readonly DateOnly? DueDate;
    public BoletoInfo? Info { get; private set; }
    public Money? PaidAmount { get; private set; }
    public DateOnly? PaidOn { get; private set; }

    public void Issue(BoletoInfo info, DateTime now)
    {
        CheckCanIssue();
        Info = info;
        SetStatus(PaymentStatus.Pending, now);
    }

    private void CheckCanIssue()
    {
        PaymentStatusTransitionValidator.CheckPreConditions(
            payment: this,
            to: PaymentStatus.Pending
        );
    }

    public void Pay(Money paidAmount, DateOnly paidOn, DateTime now)
    {
        CheckCanBePaid(paidAmount, paidOn, now);
        PaidOn = paidOn;
        PaidAmount = paidAmount;
        SetStatus(PaymentStatus.Paid, now);
    }

    private void CheckCanBePaid(Money paidAmount, DateOnly paidOn, DateTime now)
    {
        PaymentStatusTransitionValidator.CheckPreConditions(
            payment: this,
            to: PaymentStatus.Paid
        );

        if (paidAmount.IsNegative)
        {
            throw new InvalidOperationException("Paid amount cannot be negative");
        }

        if (paidAmount != Amount)
        {
            throw new InvalidOperationException("Paid amount cannot be greater than the payment amount");
        }

        var today = DateOnly.FromDateTime(now);
        if (paidOn > today)
        {
            throw new InvalidOperationException("Payment date cannot be in the future");
        }
    }

    public static PaymentBoleto Create(
        Money amount,
        DateTime now,
        DateOnly dueDate,
        Payer? payer,
        Id? id = null
    )
    {
        var paymentBoleto = new PaymentBoleto(
            id: id ?? new Id(),
            amount: amount,
            status: PaymentStatus.Creating,
            payer: payer,
            dueDate: dueDate,
            info: null,
            paidAmount: null,
            paidOn: null,
            createdAt: now,
            updatedAt: now
        );

        return paymentBoleto;
    }

    private PaymentBoleto(
        Id id,
        Money amount,
        DateOnly dueDate,
        PaymentStatus status,
        Payer? payer,
        BoletoInfo? info,
        Money? paidAmount,
        DateOnly? paidOn,
        DateTime createdAt,
        DateTime updatedAt
    ) : base(
        id: id,
        amount: amount,
        status: status,
        method: PaymentMethod.Boleto,
        payer: payer,
        createdAt: createdAt,
        updatedAt: updatedAt
    )
    {
        Info = info;
        DueDate = dueDate;
        PaidAmount = paidAmount;
        PaidOn = paidOn;
    }
}