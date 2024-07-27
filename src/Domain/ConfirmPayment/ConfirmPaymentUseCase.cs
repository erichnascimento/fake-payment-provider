using FakePaymentProvider.Domain.Payments;
using FakePaymentProvider.Domain.Services.Notification;
using FakePaymentProvider.Domain.Types.Exceptions;
using FakePaymentProvider.Library.Date;

namespace FakePaymentProvider.Domain.ConfirmPayment;

public sealed class ConfirmPaymentUseCase(
    ITimeService timeService,
    IEntityGateway entityGateway,
    INotificationService notificationService
) : IConfirmPaymentUseCase
{
    public async Task<ConfirmPaymentUseCaseResponse> Execute(ConfirmPaymentUseCaseRequest request)
    {
        var payment = await entityGateway.GetPaymentById(id: request.PaymentId);
        if (payment is null)
        {
            throw new PaymentNotFoundException(id: request.PaymentId);
        }

        return await ConfirmPayment(
            payment: payment,
            request: request
        );
    }

    private async Task<ConfirmPaymentUseCaseResponse> ConfirmPayment(
        Payment payment,
        ConfirmPaymentUseCaseRequest request
    )
    {
        var response = payment switch
        {
            PaymentBoleto paymentBoleto => await ConfirmBoletoPayment(paymentBoleto: paymentBoleto, request: request),
            _ => throw new NotImplementedException("Payment type not implemented")
        };

        await NotifyPaymentConfirmed(payment: payment);

        return response;
    }

    private async Task<ConfirmPaymentUseCaseResponse> ConfirmBoletoPayment(
        PaymentBoleto paymentBoleto,
        ConfirmPaymentUseCaseRequest request
    )
    {
        paymentBoleto.Pay(
            paidAmount: request.PaidAmount,
            paidOn: request.PaidOn ?? timeService.Today,
            now: timeService.Now
        );

        await entityGateway.SavePayment(payment: paymentBoleto);

        return new ConfirmPaymentUseCaseResponse
        {
            PaymentId = paymentBoleto.Id,
            PaymentCode = paymentBoleto.PaymentCode,
            Status = paymentBoleto.Status.ToString(),
            PaidAmount = paymentBoleto.PaidAmount!.Value,
            PaidOn = paymentBoleto.PaidOn!.Value
        };
    }

    private async Task NotifyPaymentConfirmed(Payment payment)
    {
        if (payment.PostbackUrl is null)
        {
            return;
        }

        var request = new NotifyPaymentConfirmedRequest(
            PaymentId: payment.Id,
            PaymentCode: payment.PaymentCode,
            PostbackUrl: payment.PostbackUrl
        );

        await notificationService.NotifyPaymentConfirmed(request: request);
    }
}