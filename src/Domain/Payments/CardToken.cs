namespace FakePaymentProvider.Domain.Payments;

public record CardToken(
    string Token,
    string Bin,
    string LastFour,
    string ExpirationDate
);