using FakePaymentProvider.Domain;
using FakePaymentProvider.Domain.Payments;
using FakePaymentProvider.Library.Types;

namespace FakePaymentProvider.Infra.EntityGateway.Memory;

public class InMemoryEntityGateway : IEntityGateway
{
    private readonly Dictionary<Id, Payment> _payments = new();

    public Task<Payment?> GetPaymentById(Id id)
    {
        return Task.FromResult(_payments.GetValueOrDefault(id));
    }

    public Task SavePayment(Payment payment)
    {
        _payments[payment.Id] = payment;

        return Task.CompletedTask;
    }
}