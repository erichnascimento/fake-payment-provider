using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using FakePaymentProvider.Domain.Services.Notification;

namespace FakePaymentProvider.Infra.NotificationService.Http;

public sealed class HttpNotificationService(
    HttpClient httpClient
) : INotificationService
{
    public HttpNotificationService() : this(new HttpClient())
    {
    }

    public async Task<NotifyPaymentConfirmedResponse> NotifyPaymentConfirmed(NotifyPaymentConfirmedRequest request)
    {
        var content = JsonContent.Create(
            inputValue: new NotifyPaymentConfirmedRequestBody(
                PaymentId: request.PaymentId,
                PaymentCode: request.PaymentCode
            ),
            options: new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                TypeInfoResolver = WeatherForecastContext.Default
            }
        );

        Console.WriteLine("Notifying payment confirmed to: " + request.PostbackUrl);

        await httpClient.PostAsync(
            requestUri: request.PostbackUrl,
            content: content
        );

        return new NotifyPaymentConfirmedResponse();
    }
}

public sealed record NotifyPaymentConfirmedRequestBody(
    string PaymentId,
    string PaymentCode
);

[JsonSerializable(typeof(NotifyPaymentConfirmedRequestBody))]
public partial class WeatherForecastContext : JsonSerializerContext
{
}