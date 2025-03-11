using Bookify.Application.Abstractions.Clock;

namespace Bookify.Infrastructure.Clock;
internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
    public DateTime Parse(string date) => DateTime.Parse(date);

}
