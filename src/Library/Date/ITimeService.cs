namespace FakePaymentProvider.Library.Date;

public interface ITimeService
{
    DateTime Now { get; }
}

public class SystemTimeService : ITimeService
{
    public DateTime Now => DateTime.Now;
}