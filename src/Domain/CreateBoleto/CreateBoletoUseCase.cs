using FakePaymentProvider.Domain.Payments;
using FakePaymentProvider.Domain.Types;
using FakePaymentProvider.Library.Date;

namespace FakePaymentProvider.Domain.CreateBoleto;

public class CreateBoletoUseCase(
    ITimeService timeService,
    IEntityGateway entityGateway
) : ICreateBoletoUseCase
{
    public async Task<CreateBoletoResponse> Execute(CreateBoletoRequest request)
    {
        var paymentBoleto = CreatePaymentBoleto(request: request);

        IssueBoleto(paymentBoleto: paymentBoleto);

        await entityGateway.SavePayment(payment: paymentBoleto);

        return new CreateBoletoResponse
        {
            BoletoId = paymentBoleto.Id
        };
    }

    private PaymentBoleto CreatePaymentBoleto(CreateBoletoRequest request)
    {
        var payer = CreatePayer(request);

        return PaymentBoleto.Create(
            amount: request.Amount,
            dueDate: request.DueDate,
            payer: payer,
            now: timeService.Now
        );
    }

    private void IssueBoleto(PaymentBoleto paymentBoleto)
    {
        // TODO: Implement a real BoletoInfo generator
        var boletoInfo = new BoletoInfo(
            Number: "34191.09008 63521.510047 91020.150008 5 12345678901234",
            Barcode: "34191510047910201500085012345678901234"
        );

        paymentBoleto.Issue(info: boletoInfo, now: timeService.Now);

        entityGateway.SavePayment(payment: paymentBoleto);
    }

    private static Payer? CreatePayer(CreateBoletoRequest request)
    {
        return request.HasPayerInfo
            ? new Payer(
                Name: request.PayerName,
                Email: request.PayerEmail,
                Phone: request.PayerPhoneNumber is not null
                    ? PersonalPhone.Parse(request.PayerPhoneNumber)
                    : null,
                Document: request.PayerDocument is not null
                    ? PersonalDocument.Parse(request.PayerDocument)
                    : null
            )
            : null;
    }
}