using FakePaymentProvider.Domain.Payments;
using FakePaymentProvider.Library.Types;

namespace FakePaymentProvider.Domain;

public interface IEntityGateway
{
    Task<Payment?> GetPaymentById(Id id);
    Task SavePayment(Payment payment);
}