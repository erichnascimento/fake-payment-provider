using FakePaymentProvider.Domain.Boundary.CreateBoleto;
using FakePaymentProvider.Domain.Payments;
using FakePaymentProvider.Library.Date;

namespace FakePaymentProvider.Domain;

public class CreateBoletoUseCase(
    ITimeService timeService,
    IEntityGateway entityGateway
) : ICreateBoletoUseCase
{
    public Task<CreateBoletoResponse> Execute(CreateBoletoRequest request)
    {
        var boleto = Payment.CreateBoleto(
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