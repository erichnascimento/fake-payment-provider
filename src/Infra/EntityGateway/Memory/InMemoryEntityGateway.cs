using FakePaymentProvider.Domain;
using FakePaymentProvider.Domain.Payments;
using FakePaymentProvider.Library.Types;

namespace FakePaymentProvider.Infra.EntityGateway.Memory;

public class InMemoryEntityGateway : IEntityGateway
{
    private readonly Dictionary<Id, Payment> _payments = new();

    public Payment? GetPaymentById(Id id)
    {
        return _payments.GetValueOrDefault(id);
    }

    public void SavePayment(Payment payment)
    {
        _payments[payment.Id] = payment;
    }
}