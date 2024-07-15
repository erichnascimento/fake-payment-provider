using FakePaymentProvider.Domain.Boundary.CreateBoleto;
using FakePaymentProvider.Domain.Payments;
using FakePaymentProvider.Library.Date;

namespace FakePaymentProvider.Domain;

public class CreateBoletoUseCase(
    CreateBoletoRequest request,
    ITimeService timeService,
    IEntityGateway entityGateway
) : ICreateBoletoUseCase
{
    public Task<CreateBoletoResponse> Execute()
    {
        var boleto = Payment.Create(
            method: PaymentMethod.Boleto,
            amount: request.Amount,
            now: timeService.Now
        );

        entityGateway.SavePayment(payment: boleto);

        return Task.FromResult(new CreateBoletoResponse
        {
            BoletoId = boleto.Id
        });
    }
}