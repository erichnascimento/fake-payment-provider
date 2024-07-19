using FakePaymentProvider.Domain.Types;

namespace FakePaymentProvider.Domain.Payments;

public record Payer(
    string? Name,
    string? Email,
    PersonalPhone? Phone,
    PersonalDocument? Document
);