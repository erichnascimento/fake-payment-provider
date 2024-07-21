namespace WebApiApplication.HttpRestApi.Handles.CreatePayment.Types;

public sealed record PayerDocument
{
    public string? Type;
    public string? Number;
}