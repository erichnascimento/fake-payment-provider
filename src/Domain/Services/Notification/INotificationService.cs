using FakePaymentProvider.Library.Types;

namespace FakePaymentProvider.Domain.Services.Notification;

public interface INotificationService
{
    Task<NotifyPaymentConfirmedResponse> NotifyPaymentConfirmed(NotifyPaymentConfirmedRequest request);
}

public sealed record NotifyPaymentConfirmedRequest(
    Id PaymentId,
    string PaymentCode,
    Uri PostbackUrl
);

public sealed record NotifyPaymentConfirmedResponse;