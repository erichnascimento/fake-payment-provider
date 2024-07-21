namespace WebApiApplication.HttpRestApi.Handles.CreatePayment.Types;

public sealed record Payer
{
    public string? Name;
    public string? Email;
    public string? CellPhoneNumber;
    public PayerDocument? Document;
    public PayerAddress? Address;
    public PayerCard? Card;
}