namespace FakePaymentProvider.Domain.Payments;

public record PixInfo(
    string Key,
    string CopyAndPaste,
    string QrCodeBase64,
    DateTime ExpireAt,
    DateOnly DueDate
);