namespace FakePaymentProvider.Library.Date;

public interface ITimeService
{
    DateTime Now { get; }
    DateOnly Today { get; }
}

public class SystemTimeService : ITimeService
{
    public DateTime Now => DateTime.Now;
    public DateOnly Today => DateOnly.FromDateTime(DateTime.Today);
}