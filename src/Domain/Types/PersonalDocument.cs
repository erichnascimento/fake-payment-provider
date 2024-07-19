using FakePaymentProvider.Library;

namespace FakePaymentProvider.Domain.Types;

public record PersonalDocument
{
    public readonly string Number;
    public readonly PersonalDocumentType Type;

    public static PersonalDocument CreateCpf(string number)
    {
        return new PersonalDocument(
            number: number,
            type: PersonalDocumentType.Cpf
        );
    }

    public static PersonalDocument CreateCnpj(string number)
    {
        return new PersonalDocument(
            number: number,
            type: PersonalDocumentType.Cnpj
        );
    }

    public PersonalDocument(
        string number,
        PersonalDocumentType type
    )
    {
        Validate(number, type);

        Number = number;
        Type = type;
    }

    private static void Validate(
        string number,
        PersonalDocumentType type
    )
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            throw new ArgumentException("Number is required");
        }

        switch (type)
        {
            case PersonalDocumentType.Cpf:
                if (number.Length != 11)
                {
                    throw new ArgumentException("CPF must have 11 digits");
                }

                break;
            case PersonalDocumentType.Cnpj:
                if (number.Length != 14)
                {
                    throw new ArgumentException("CNPJ must have 14 digits");
                }

                break;
            default:
                throw new ArgumentException("Document type is invalid");
        }
    }

    public static PersonalDocument Parse(string value)
    {
        var sanitizedDocument = Strings.OnlyNumbers(input: value);
        
        return sanitizedDocument.Length switch
        {
            11 => CreateCpf(number: sanitizedDocument),
            14 => CreateCnpj(number: sanitizedDocument),
            _ => throw new ArgumentException("Invalid document number")
        };
    }
}