namespace FakePaymentProvider.Library.Types;

public readonly struct Money(decimal amount, Currency currency)
{
    public decimal Amount { get; } = CheckIsValidAmount(amount, currency);
    public Currency Currency { get; } = currency;
    public bool IsZero => Amount is 0;
    public bool IsNegative => Amount < 0;
    public bool IsPositive => Amount > 0;
    public bool IsNotPositive => IsPositive is false;

    private static decimal CheckIsValidAmount(decimal amount, Currency currency)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than or equal to zero");
        }

        return amount;
    }
}