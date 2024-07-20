namespace FakePaymentProvider.Domain.CreateBoleto;

public interface ICreateBoletoUseCase
{
    Task<CreateBoletoResponse> Execute(CreateBoletoRequest request);
}