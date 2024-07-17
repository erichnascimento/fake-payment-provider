using FakePaymentProvider.Domain.Payments;
using FakePaymentProvider.Library.Types;

namespace FakePaymentProvider.Domain;

public interface IEntityGateway
{
    Payment? GetPaymentById(Id id);
    void SavePayment(Payment payment);
}