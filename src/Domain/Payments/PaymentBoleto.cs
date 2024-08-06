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
            throw new InvalidOperationException("Paid amount cannot be different from the payment amount");
        }

        var today = DateOnly.FromDateTime(now);
        if (paidOn > today)
        {
            throw new InvalidOperationException("Payment date cannot be in the future");
        }
    }

    public static PaymentBoleto Create(
        string paymentCode,
        Money amount,
        DateTime now,
        DateOnly dueDate,
        Payer? payer,
        Uri? postbackUrl = null,
        Id? id = null
    )
    {
        var paymentBoleto = new PaymentBoleto(
            id: id ?? new Id(),
            paymentCode: paymentCode,
            amount: amount,
            status: PaymentStatus.Creating,
            payer: payer,
            postbackUrl: postbackUrl,
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
        string paymentCode,
        Money amount,
        DateOnly dueDate,
        PaymentStatus status,
        Payer? payer,
        Uri? postbackUrl,
        BoletoInfo? info,
        Money? paidAmount,
        DateOnly? paidOn,
        DateTime createdAt,
        DateTime updatedAt
    ) : base(
        id: id,
        paymentCode: paymentCode,
        amount: amount,
        status: status,
        method: PaymentMethod.Boleto,
        payer: payer,
        postbackUrl: postbackUrl,
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