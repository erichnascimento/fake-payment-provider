namespace FakePaymentProvider.Domain.Payments;

public static class PaymentStatusTransitionValidator
{
    public static void CheckIsValidTransition(
        PaymentStatus from,
        PaymentStatus to
    )
    {
        if (IsValidTransition(from, to))
        {
            return;
        }

        if (from is PaymentStatus.Creating && to is not PaymentStatus.Pending)
        {
            throw new InvalidOperationException("Payment must be pending after creation");
        }

        // TODO: Add more specific error messages in order to help understand what went wrong

        throw new InvalidOperationException($"Invalid transition from {from} to {to}");
    }

    private static bool IsValidTransition(
        PaymentStatus from,
        PaymentStatus to
    )
    {
        return (from, to) switch
        {
            (PaymentStatus.Creating, PaymentStatus.Pending) => true,
            (PaymentStatus.Pending, PaymentStatus.Paid) => true,
            (PaymentStatus.Pending, PaymentStatus.Cancelled) => true,
            _ => false
        };
    }

    public static void CheckPreConditions(Payment payment, PaymentStatus to)
    {
        if (to is PaymentStatus.Creating)
        {
            throw new InvalidOperationException("Payment cannot be in Creating state");
        }

        if (to is PaymentStatus.Pending)
        {
            if (payment.Amount.IsNotPositive)
            {
                throw new InvalidOperationException("Amount must be greater than 0");
            }
        }
    }
}