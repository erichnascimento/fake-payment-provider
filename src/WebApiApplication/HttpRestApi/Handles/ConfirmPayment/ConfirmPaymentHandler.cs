using FakePaymentProvider.Domain.ConfirmPayment;
using FakePaymentProvider.Library.Types;
using Microsoft.AspNetCore.Mvc;
using WebApiApplication.HttpRestApi.Handles.Base;

namespace WebApiApplication.HttpRestApi.Handles.ConfirmPayment;

public sealed class ConfirmPaymentHandler(
    IConfirmPaymentUseCase confirmPaymentUseCase
) : BaseHandler<ConfirmPaymentRequest, ConfirmPaymentResponse>
{
    protected override async Task<ConfirmPaymentResponse> DoHandle([FromBody] ConfirmPaymentRequest request)
    {
        var useCaseRequest = new ConfirmPaymentUseCaseRequest
        {
            PaymentId = request.PaymentId,
            PaidAmount = new Money(
                value: request.PaidAmount,
                currency: request.Currency
            ),
            PaidOn = request.PaidOn
        };

        var response = await confirmPaymentUseCase.Execute(request: useCaseRequest);

        return new ConfirmPaymentResponse
        {
            Id = response.PaymentId,
            PaymentCode = response.PaymentCode,
            Status = response.Status,
            PaidAmount = response.PaidAmount.Value,
            PaidOn = response.PaidOn
        };
    }
}