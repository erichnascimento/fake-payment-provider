namespace FakePaymentProvider.Library.Types;

public readonly struct Id(Guid value) : IEquatable<Id>, IComparable<Id>, IComparable
{
    private readonly Guid _value = value;
    public static Id New() => new(Guid.NewGuid());
    public static Id Parse(string value) => new(Guid.Parse(value));

    public override string ToString() => _value.ToString();

    public bool Equals(Id other)
    {
        return _value.Equals(other._value);
    }

    public override bool Equals(object? obj)
    {
        return obj is Id other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public int CompareTo(Id other)
    {
        return _value.CompareTo(other._value);
    }
    
    public int CompareTo(object? obj)
    {
        if (obj is null) return 1;
        if (obj is not Id other) throw new ArgumentException("Object is not an Id");
        return CompareTo(other);
    }

    public static bool operator ==(Id left, Id right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Id left, Id right)
    {
        return !(left == right);
    }
    
    public static implicit operator Guid(Id id) => id._value;
    public static implicit operator Id(Guid value) => new(value);
    
    public static implicit operator string(Id id) => id._value.ToString();
    public static implicit operator Id(string value) => new(Guid.Parse(value));
}