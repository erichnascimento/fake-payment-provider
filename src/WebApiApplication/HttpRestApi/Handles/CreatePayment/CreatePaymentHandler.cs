using FakePaymentProvider.Domain.CreateBoleto;
using FakePaymentProvider.Library.Types;
using Microsoft.AspNetCore.Mvc;
using WebApiApplication.HttpRestApi.Handles.Base;

namespace WebApiApplication.HttpRestApi.Handles.CreatePayment;

public sealed class CreatePaymentHandler(
    ICreateBoletoUseCase createBoletoUseCase
) : BaseHandler<CreatePaymentRequest, CreatePaymentResponse>
{
    protected override async Task<CreatePaymentResponse> DoHandle([FromBody] CreatePaymentRequest request)
    {
        return request.PaymentMethod switch
        {
            "boleto" => await HandleCreateBoleto(request),
            _ => throw new ArgumentException("Invalid payment method")
        };
    }
    
    private async Task<CreatePaymentResponse> HandleCreateBoleto(CreatePaymentRequest request)
    {
        var createBoletoRequest = BuildCreateBoletoRequest(request: request);
        var response = await createBoletoUseCase.Execute(request: createBoletoRequest);

        return new CreatePaymentResponse
        {
            Id = response.BoletoId
        };
    }
    
    private static CreateBoletoRequest BuildCreateBoletoRequest(CreatePaymentRequest request)
    {
        var payerDocument = request.Payer?.Document?.Type switch
        {
            "CPF" or "CNPJ"  => request.Payer.Document.Number,
            _ => throw new ArgumentException("Invalid document type for bolero payer")
        };
        
        var dueDate = request.DueDate ?? throw new ArgumentException("DueDate is required for boleto");

        return new CreateBoletoRequest
        {
            Amount = new Money(
                value: request.Amount,
                currency: new Currency(request.Currency)
            ),
            DueDate = dueDate,
            PayerName = request.Payer?.Name,
            PayerEmail = request.Payer?.Email,
            PayerPhoneNumber = request.Payer?.CellPhoneNumber,
            PayerDocument = payerDocument
        };
    }
}