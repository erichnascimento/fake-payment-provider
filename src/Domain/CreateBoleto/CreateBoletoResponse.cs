using FakePaymentProvider.Library.Types;

namespace FakePaymentProvider.Domain.CreateBoleto;

public sealed record CreateBoletoResponse
{
    public required Id BoletoId { get; init; }
    public required string PaymentCode { get; init; }
    public required string Status { get; init; }
    public required BoletoInfo Boleto { get; init; }
    public PixInfo? Pix { get; init; }
}

public sealed record BoletoInfo
{
    public required string Number { get; init; }
    public required string Barcode { get; init; }
    public DateOnly DueDate { get; init; }
}

public sealed record PixInfo
{
    public required string CopyPaste { get; init; }
    public required string QrCodeBase64 { get; init; }
    public DateOnly DueDate { get; init; }
}