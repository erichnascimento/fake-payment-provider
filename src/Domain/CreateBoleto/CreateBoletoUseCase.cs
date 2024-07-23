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
        var paymentBoleto = CreateAndIssuePaymentBoleto(request: request);
        await entityGateway.SavePayment(payment: paymentBoleto);

        return CreateResponse(payment: paymentBoleto);
    }

    private PaymentBoleto CreateAndIssuePaymentBoleto(CreateBoletoRequest request)
    {
        var paymentBoleto = CreatePaymentBoleto(request: request);
        IssueBoleto(paymentBoleto: paymentBoleto);

        return paymentBoleto;
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
        var boletoInfo = new Payments.BoletoInfo(
            Number: "34191.09008 63521.510047 91020.150008 5 12345678901234",
            Barcode: "34191510047910201500085012345678901234"
        );

        paymentBoleto.Issue(info: boletoInfo, now: timeService.Now);
    }

    private static Payer? CreatePayer(CreateBoletoRequest request)
    {
        if (request.HasPayerInfo is not true)
        {
            return null;
        }

        return new Payer(
            Name: request.PayerName,
            Email: request.PayerEmail,
            Phone: request.PayerPhoneNumber is not null
                ? PersonalPhone.Parse(request.PayerPhoneNumber)
                : null,
            Document: request.PayerDocument is not null
                ? PersonalDocument.Parse(request.PayerDocument)
                : null
        );
    }

    private static CreateBoletoResponse CreateResponse(PaymentBoleto payment)
    {
        if (payment.Info is null)
        {
            throw new InvalidOperationException("Boleto info is missing");
        }

        var boletoInfo = new BoletoInfo
        {
            Number = payment.Info.Number,
            Barcode = payment.Info.Barcode,
            DueDate = payment.DueDate ?? throw new InvalidOperationException("DueDate is required")
        };

        return new CreateBoletoResponse
        {
            BoletoId = payment.Id,
            Status = payment.Status.ToString(),
            Boleto = boletoInfo
        };
    }
}