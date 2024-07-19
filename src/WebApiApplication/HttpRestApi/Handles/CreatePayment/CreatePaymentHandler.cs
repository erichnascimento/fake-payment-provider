using FakePaymentProvider.Domain.Boundary.CreateBoleto;
using FakePaymentProvider.Library.Types;
using Microsoft.AspNetCore.Mvc;
using WebApiApplication.HttpRestApi.Handles.Base;

namespace WebApiApplication.HttpRestApi.Handles.CreatePayment;

public class CreatePaymentHandler(
    ICreateBoletoUseCase createBoletoUseCase
) : BaseHandler<CreatePaymentRequest, CreatePaymentResponse>
{
    protected override async Task<CreatePaymentResponse> DoHandle([FromBody] CreatePaymentRequest request)
    {
        if (request.IsBoleto is not true)
        {
            throw new NotImplementedException("Payment method not implemented");
        }

        var createBoletoRequest = new CreateBoletoRequest
        {
            Amount = new Money(
                value: request.Amount,
                currency: new Currency(request.Currency)
            ),
            DueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(3))
        };

        var response = await createBoletoUseCase.Execute(request: createBoletoRequest);

        return new CreatePaymentResponse
        {
            Id = response.BoletoId
        };
    }
}