namespace FakePaymentProvider.Domain.Payments;

public record Card(
    string HolderName,
    string Number,
    string ExpirationDate,
    string SecurityCode,
    CardScheme Scheme
);