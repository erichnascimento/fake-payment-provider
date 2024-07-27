using System.Text.Json.Serialization;
using WebApiApplication.HttpRestApi.Handles.Base;

namespace WebApiApplication.HttpRestApi.Handles.CreatePayment;

public sealed record CreatePaymentResponse : CreatedResponse
{
    public required string Id;
    public required string PaymentCode;
    public required string Status;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BoletoInfo? Boleto;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PixInfo? Pix;

    protected override string Uri => $"/v1/payments/{Id}";

    public override string ToLoggableString()
    {
        return $"Id={Id}";
    }
}

public sealed record BoletoInfo
{
    public required string Number;
    public required string Barcode;
    public required DateOnly DueDate;
}

public sealed record PixInfo
{
    public required string CopyPaste;
    public required string QrCodeBase64;
    public required string DueDate;
}