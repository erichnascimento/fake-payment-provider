namespace WebApiApplication.HttpRestApi;

/// <summary>
/// This interface is used to configure the host of the application.
/// It is useful for adding new services, configuration sources, logging providers, and more,
/// specially in the testing environment where you want to mock the services, configuration sources, logging providers, etc.
/// </summary>
public interface IApplicationConfigurator
{
    public void ConfigureHost(IHostApplicationBuilder builder);
}