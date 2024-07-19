namespace FakePaymentProvider.Domain.Types;

public record Address(
    string Street,
    string Number,
    string Neighborhood,
    string City,
    string State,
    string ZipCode,
    string? Complement = null,
    string? Country = null
);