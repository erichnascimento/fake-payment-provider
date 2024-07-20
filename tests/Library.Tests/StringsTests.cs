namespace FakePaymentProvider.Library.Tests;

public class StringsTests
{
    [Test]
    [TestCase("abc123def456", "123456")]
    [TestCase("123456", "123456")]
    [TestCase("", "")]
    [TestCase("abc", "")]
    [TestCase("  123  ", "123")]
    [TestCase("  123  456  ", "123456")]
    [TestCase("?>%@*_abc123def4 56.9", "1234569")]
    public void TestOnlyNumbers(string input, string expected)
    {
        // Act
        var result = Strings.OnlyNumbers(input);

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }
}