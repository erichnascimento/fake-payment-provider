namespace FakePaymentProvider.Domain.ConfirmPayment;

public interface IConfirmPaymentUseCase
{
    Task<ConfirmPaymentUseCaseResponse> Execute(ConfirmPaymentUseCaseRequest request);
}