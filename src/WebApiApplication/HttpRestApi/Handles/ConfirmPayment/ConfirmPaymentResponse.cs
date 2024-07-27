using WebApiApplication.HttpRestApi.Handles.Base;

namespace WebApiApplication.HttpRestApi.Handles.ConfirmPayment;

public sealed record ConfirmPaymentResponse : OkResponse
{
    public required string Id;
    public required string PaymentCode;
    public required string Status;
    public required decimal PaidAmount;
    public DateOnly? PaidOn;

    public override string ToLoggableString()
    {
        return $"Id={Id}, PaymentCode={PaymentCode}, Status={Status}, PaidAmount={PaidAmount}, PaidOn={PaidOn}";
    }
}
