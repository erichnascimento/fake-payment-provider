namespace FakePaymentProvider.Library.Types;

public readonly struct Money(decimal value, Currency currency)
{
    public decimal Value { get; } = ValidateAndReturnOrCry(value, currency);
    public Currency Currency { get; } = currency;
    public bool IsZero => Value is 0;
    public bool IsNegative => Value < 0;
    public bool IsPositive => Value > 0;
    public bool IsNotPositive => IsPositive is false;

    public static Money Zero(Currency currency) => new(0, currency);

    public static Money operator +(Money left, Money right)
    {
        CheckSameCurrency(left.Currency, right.Currency);

        return new Money(left.Value + right.Value, left.Currency);
    }

    public static Money operator -(Money left, Money right)
    {
        CheckSameCurrency(left.Currency, right.Currency);

        return new Money(left.Value - right.Value, left.Currency);
    }

    public static Money operator *(Money left, decimal right)
    {
        return new Money(left.Value * right, left.Currency);
    }

    public static Money operator /(Money left, decimal right)
    {
        return new Money(left.Value / right, left.Currency);
    }

    public static bool operator ==(Money left, Money right)
    {
        return left.Value == right.Value && left.Currency == right.Currency;
    }

    public static bool operator !=(Money left, Money right)
    {
        return !(left == right);
    }

    public override bool Equals(object? obj)
    {
        return obj is Money money && this == money;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value, Currency);
    }

    public override string ToString()
    {
        return $"{Value} {Currency}";
    }

    private static decimal ValidateAndReturnOrCry(decimal value, Currency currency)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Value must be greater than or equal to zero");
        }

        return value;
    }

    private static void CheckSameCurrency(Currency left, Currency right)
    {
        if (left != right)
        {
            throw new InvalidOperationException("Cannot operate with money with different currencies");
        }
    }

    public static Money NewBrl(decimal value)
    {
        return new Money(
            value: value,
            currency: Currency.Brl
        );
    }
}