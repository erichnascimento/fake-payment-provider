namespace FakePaymentProvider.Library.Types;

public readonly struct Currency : IEquatable<Currency>, IComparable<Currency>, IComparable
{
    private readonly string _code;
    public static Currency Brl => new("BRL");
    public static Currency Usd => new("USD");
    public static Currency Eur => new("EUR");

    public Currency(string code)
    {
        Validate(code);
        _code = code;
    }

    private static void Validate(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Currency code cannot be null or empty", nameof(code));
        }
        
        if (code.Length != 3)
        {
            throw new ArgumentException("Currency code must have 3 characters", nameof(code));
        }

        switch (code)
        {
            case "BRL":
            case "USD":
            case "EUR":
                break;
            default:
                throw new ArgumentException("Currency code is invalid", nameof(code));
        }
    }

    public bool Equals(Currency other)
    {
        return _code == other._code;
    }

    public int CompareTo(Currency other)
    {
        return string.Compare(_code, other._code, StringComparison.Ordinal);
    }

    public int CompareTo(object? obj)
    {
        if (obj is null) return 1;
        if (obj is not Currency other) throw new ArgumentException("Object is not a Currency");
        return CompareTo(other);
    }

    public override bool Equals(object? obj)
    {
        return obj is Currency other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _code.GetHashCode();
    }

    public static bool operator ==(Currency left, Currency right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Currency left, Currency right)
    {
        return !(left == right);
    }

    public static implicit operator string(Currency currency) => currency._code;
    public static implicit operator Currency(string code) => new(code);

    public override string ToString() => _code;
}