namespace FakePaymentProvider.Domain.Payments;

public record BoletoInfo(
    string Number,
    string Barcode
);