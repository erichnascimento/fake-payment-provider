using FakePaymentProvider.Domain;
using FakePaymentProvider.Domain.Boundary.CreateBoleto;
using FakePaymentProvider.Library.Date;
using FakePaymentProvider.Library.Types;
using Microsoft.AspNetCore.Mvc;
using WebApiApplication.HttpRestApi.Handles.Base;

namespace WebApiApplication.HttpRestApi.Handles.CreatePayment;

public class CreatePaymentHandler(
    IServiceProvider serviceProvider
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
                amount: request.Amount,
                currency: new Currency(request.Currency)
            ),

            // TODO: Read from request

            CustomerName = "John Doe",
            CustomerEmail = "johndoe@example.com",
            DueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(3))
        };

        var useCase = new CreateBoletoUseCase(
            request: createBoletoRequest,
            timeService: serviceProvider.GetRequiredService<ITimeService>(),
            entityGateway: serviceProvider.GetRequiredService<IEntityGateway>()
        );

        var response = await useCase.Execute();

        return new CreatePaymentResponse
        {
            Id = response.BoletoId
        };
    }
}