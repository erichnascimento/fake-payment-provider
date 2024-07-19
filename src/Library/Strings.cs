namespace FakePaymentProvider.Library;

public static class Strings
{
    public static string OnlyNumbers(string input)
    {
        return new string(input.Where(char.IsDigit).ToArray());
    }
}