using FakePaymentProvider.Library.Types;

namespace FakePaymentProvider.Domain.Types.Exceptions;

public class PaymentNotFoundException(Id id) : Exception("Payment not found.")
{
}