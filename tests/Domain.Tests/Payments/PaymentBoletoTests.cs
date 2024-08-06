using FakePaymentProvider.Domain.Payments;
using FakePaymentProvider.Domain.Types;
using FakePaymentProvider.Library.Types;

namespace FakePaymentProvider.Domain.Tests.Payments;

[TestFixture]
public class PaymentBoletoTests
{
    [Test]
    public void TestCreatePaymentBoleto_WhenValid_ShouldCreatePaymentBoleto()
    {
        // Arrange
        var now = DateTime.UtcNow;

        var expectedPaymentId = new Id();
        var expectedPayer = CreateThatPayerForTest();
        var expectedDueDate = DateOnly.FromDateTime(now).AddDays(5);

        // Act
        var paymentBoleto = CreateThatPaymentBoletoForTest(
            now: now,
            paymentId: expectedPaymentId
        );

        // Arrange
        Assert.Multiple(() =>
        {
            Assert.That(paymentBoleto.Id, Is.EqualTo(expectedPaymentId));
            Assert.That(paymentBoleto.PaymentCode, Is.EqualTo("abc123"));
            Assert.That(paymentBoleto.Amount.Currency, Is.EqualTo(Currency.Brl));
            Assert.That(paymentBoleto.Amount.Value, Is.EqualTo(100));
            Assert.That(paymentBoleto.Status, Is.EqualTo(PaymentStatus.Creating));
            Assert.That(paymentBoleto.Method, Is.EqualTo(PaymentMethod.Boleto));
            Assert.That(paymentBoleto.Payer, Is.EqualTo(expectedPayer));
            Assert.That(paymentBoleto.PostbackUrl, Is.EqualTo(new Uri("https://example.com/postback")));
            Assert.That(paymentBoleto.CreatedAt, Is.EqualTo(now));
            Assert.That(paymentBoleto.UpdatedAt, Is.EqualTo(now));
            Assert.That(paymentBoleto.Info, Is.Null);
            Assert.That(paymentBoleto.DueDate, Is.EqualTo(expectedDueDate));
            Assert.That(paymentBoleto.PaidAmount, Is.Null);
            Assert.That(paymentBoleto.PaidOn, Is.Null);
        });
    }

    [Test]
    public void TestIssue_WhenValid_ShouldIssuePaymentBoleto()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var paymentBoleto = CreateThatPaymentBoletoForTest(now: now);
        var expectedInfo = CreateThatBoletoInfoForTest();

        // Act
        paymentBoleto.Issue(info: expectedInfo, now: now);

        // Arrange
        Assert.Multiple(() =>
        {
            Assert.That(paymentBoleto.Info, Is.EqualTo(expectedInfo));
            Assert.That(paymentBoleto.Status, Is.EqualTo(PaymentStatus.Pending));
            Assert.That(paymentBoleto.UpdatedAt, Is.EqualTo(now));
        });
    }

    [Test]
    public void TestIssue_WhenAlreadyIssued_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var paymentBoleto = CreateThatPaymentBoletoForTest(now: now);
        var boletoInfo = new BoletoInfo(
            Number: "12345678901234",
            DigitableLine: "34191.09008 63521.510047 91020.150008 5 12345678901234",
            Barcode: "12345678901234567890123456789012345678901234567"
        );
        paymentBoleto.Issue(info: boletoInfo, now: now);

        // Act
        var issueAction = () => paymentBoleto.Issue(info: boletoInfo, now: now);

        // Arrange
        Assert.That(issueAction, Throws.InvalidOperationException);
    }

    [Test]
    public void TestPay_WhenValid_ShouldPayPaymentBoleto()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var paymentBoleto = CreateThatIssuedPaymentBoletoForTest(now: now);
        var expectedPaidAmount = Money.NewBrl(100);
        var expectedPaidOn = DateOnly.FromDateTime(now);

        // Act
        paymentBoleto.Pay(
            paidAmount: expectedPaidAmount,
            paidOn: expectedPaidOn,
            now: now
        );

        // Arrange
        Assert.Multiple(() =>
        {
            Assert.That(paymentBoleto.PaidAmount, Is.EqualTo(expectedPaidAmount));
            Assert.That(paymentBoleto.PaidOn, Is.EqualTo(expectedPaidOn));
            Assert.That(paymentBoleto.Status, Is.EqualTo(PaymentStatus.Paid));
            Assert.That(paymentBoleto.UpdatedAt, Is.EqualTo(now));
        });
    }

    [Test]
    public void TestPay_WhenAlreadyPaid_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var paymentBoleto = CreateThatAlreadyPaidPaymentBoletoForTest(now: now);
        var paidAmount = Money.NewBrl(100);
        var paidOn = paymentBoleto.DueDate!.Value;

        // Act
        var action = () => paymentBoleto.Pay(
            paidAmount: paidAmount,
            paidOn: paidOn,
            now: now
        );

        // Arrange
        Assert.That(action, Throws.InvalidOperationException);
    }

    private static Payer CreateThatPayerForTest()
    {
        return new Payer(
            Name: "John Doe",
            Email: "johndoe@example.com",
            Phone: PersonalPhone.NewMobile(
                areaCode: "41",
                number: "987654321",
                countryCode: "55"
            ),
            Document: PersonalDocument.CreateCpf(number: "12345678909")
        );
    }

    private static PaymentBoleto CreateThatPaymentBoletoForTest(
        DateTime now,
        Id? paymentId = null
    )
    {
        var payer = CreateThatPayerForTest();
        var today = DateOnly.FromDateTime(now);

        return PaymentBoleto.Create(
            paymentCode: "abc123",
            amount: Money.NewBrl(100),
            dueDate: today.AddDays(5),
            payer: payer,
            now: now,
            id: paymentId,
            postbackUrl: new Uri("https://example.com/postback")
        );
    }

    private static BoletoInfo CreateThatBoletoInfoForTest()
    {
        return new BoletoInfo(
            Number: "12345678901234",
            DigitableLine: "34191.09008 63521.510047 91020.150008 5 12345678901234",
            Barcode: "12345678901234567890123456789012345678901234567"
        );
    }

    private static PaymentBoleto CreateThatIssuedPaymentBoletoForTest(
        DateTime now,
        Id? paymentId = null
    )
    {
        var paymentBoleto = CreateThatPaymentBoletoForTest(
            now: now,
            paymentId: paymentId
        );

        var boletoInfo = CreateThatBoletoInfoForTest();

        paymentBoleto.Issue(
            info: boletoInfo,
            now: now
        );

        return paymentBoleto;
    }

    private static PaymentBoleto CreateThatAlreadyPaidPaymentBoletoForTest(
        DateTime now,
        Id? paymentId = null
    )
    {
        var paymentBoleto = CreateThatIssuedPaymentBoletoForTest(
            now: now,
            paymentId: paymentId
        );

        // advance the date to simulate a payment on the due date
        now = paymentBoleto.DueDate!.Value.ToDateTime(new TimeOnly());
        var today = DateOnly.FromDateTime(now);

        var paidAmount = Money.NewBrl(100);
        var paidOn = today;

        paymentBoleto.Pay(
            paidAmount: paidAmount,
            paidOn: paidOn,
            now: now
        );

        return paymentBoleto;
    }
}