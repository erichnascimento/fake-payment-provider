namespace WebApiApplication.HttpRestApi.Handles.CreatePayment.Types;

public sealed record PayerCard
{
    public required string HolderName;
    public required string Number;
    public required string Expiration;
    public required string SecurityCode;
    public bool? SaveCard;
}