using System.Text.Json;
using FakePaymentProvider.Domain;
using FakePaymentProvider.Domain.CreateBoleto;
using FakePaymentProvider.Infra.EntityGateway.Memory;
using FakePaymentProvider.Library.Date;
using WebApiApplication.HttpRestApi.Handles.CreatePayment;

namespace WebApiApplication.HttpRestApi;

public class DefaultApplicationConfigurator : IApplicationConfigurator
{
    public void ConfigureHost(IHostApplicationBuilder builder)
    {
        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);

            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.IncludeFields = true;
        });

        builder.Services.AddSingleton<ITimeService, SystemTimeService>();
        builder.Services.AddSingleton<IEntityGateway, InMemoryEntityGateway>();
        builder.Services.AddSingleton<ICreateBoletoUseCase, CreateBoletoUseCase>();
        builder.Services.AddSingleton<CreatePaymentHandler>();
    }
}