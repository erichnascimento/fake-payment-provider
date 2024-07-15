namespace FakePaymentProvider.Domain.Boundary.CreateBoleto;

public interface ICreateBoletoUseCase
{
    Task<CreateBoletoResponse> Execute();
}