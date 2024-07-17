using WebApiApplication.HttpRestApi.Handles.Base;

namespace WebApiApplication.HttpRestApi.Handles.CreatePayment;

public sealed record CreatePaymentResponse : CreatedResponse
{
    public required string Id;
    protected override string Uri => $"/v1/payments/{Id}";

    public override string ToLoggableString()
    {
        return $"Id={Id}";
    }
}