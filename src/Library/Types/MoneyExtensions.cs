namespace FakePaymentProvider.Library.Types;

public static class MoneyExtensions
{
    public static Money Add(this Money money, Money other)
    {
        CheckSameCurrency(money, other);

        return new Money(money.Amount + other.Amount, money.Currency);
    }

    public static Money Subtract(this Money money, Money other)
    {
        CheckSameCurrency(money, other);

        return new Money(money.Amount - other.Amount, money.Currency);
    }
    
    private static void CheckSameCurrency(Money money, Money other)
    {
        if (money.Currency != other.Currency)
        {
            throw new InvalidOperationException("Cannot operate on money with different currencies");
        }
    }
}