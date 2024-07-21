namespace FakePaymentProvider.Domain.Types;

public record PersonalPhone(
    string AreaCode,
    string Number,
    string CountryCode,
    PersonalPhoneType Type,
    bool? IsWhatsApp = false
)
{
    public string FullNumber => $"+{CountryCode}{AreaCode}{Number}";

    public static PersonalPhone NewMobile(
        string areaCode,
        string number,
        string countryCode,
        bool isWhatsApp = false
    ) => new(
        areaCode,
        number,
        countryCode,
        PersonalPhoneType.Mobile,
        isWhatsApp
    );

    public static PersonalPhone Parse(string fullNumber)
    {
        if (fullNumber.StartsWith('+'))
        {
            fullNumber = fullNumber[1..];
        }

        if (fullNumber.StartsWith("55") is not true)
        {
            throw new NotImplementedException("Only Brazilian phone numbers are supported");
        }

        if (fullNumber.Length is 13)
        {
            // mobile number
            var areaCode = fullNumber[2..4];
            var number = fullNumber[4..];

            return new PersonalPhone(
                AreaCode: areaCode,
                Number: number,
                CountryCode: "55",
                Type: PersonalPhoneType.Mobile
            );
        }

        if (fullNumber.Length is 12)
        {
            // landline number
            var areaCode = fullNumber[2..4];
            var number = fullNumber[4..];

            return new PersonalPhone(
                AreaCode: areaCode,
                Number: number,
                CountryCode: "55",
                Type: PersonalPhoneType.Landline
            );
        }

        throw new ArgumentException("Invalid phone number", nameof(fullNumber));
    }
}