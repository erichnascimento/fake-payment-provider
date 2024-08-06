namespace WebApiApplication.HttpRestApi.Handles.CreatePayment.Types;

public sealed record PayerAddress
{
    public required string Street;
    public required string Number;
    public string? Complement;
    public required string Neighborhood;
    public required string City;
    public required string State;
    public required string PostalCode;
    public required string Country;
}