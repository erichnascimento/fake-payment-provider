using Microsoft.AspNetCore.Mvc;
using WebApiApplication.HttpRestApi.Handles.Base;

namespace WebApiApplication.HttpRestApi.Handles.ConfirmPayment;

public sealed record ConfirmPaymentRequest : BaseRequest
{
    [FromRoute(Name = "id")] public required string PaymentId { get; init; }
    public required decimal PaidAmount;
    public required string Currency;
    public DateOnly? PaidOn;

    public override void Validate()
    {
        // TODO: extract this validation to a separate class

        if (PaidAmount <= 0)
        {
            throw new ArgumentException("PaidAmount must be greater than zero");
        }
    }

    public override string ToLoggableString()
    {
        return $"PaymentId={PaymentId}, PaidAmount={PaidAmount}, Currency={Currency} PaidOn={PaidOn}";
    }
}