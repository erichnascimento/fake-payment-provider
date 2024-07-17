namespace FakePaymentProvider.Library.Types;

public readonly struct Currency(string code) : IEquatable<Currency>, IComparable<Currency>, IComparable
{
    public string Code { get; } = ValidateAndReturnOrCry(code);

    public static string ValidateAndReturnOrCry(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Currency code cannot be null or empty", nameof(code));
        }

        return code;
    }

    public bool Equals(Currency other)
    {
        return Code == other.Code;
    }

    public int CompareTo(Currency other)
    {
        return string.Compare(Code, other.Code, StringComparison.Ordinal);
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
        return Code.GetHashCode();
    }

    public static bool operator ==(Currency left, Currency right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Currency left, Currency right)
    {
        return !(left == right);
    }

    public static implicit operator string(Currency currency) => currency.Code;
    public static implicit operator Currency(string code) => new(code);

    public override string ToString() => Code;
}