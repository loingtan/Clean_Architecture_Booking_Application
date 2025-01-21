using Bookify.Domain.Entities.Abstractions;

namespace Bookify.Domain.Entities.Bookings.ValueObjects;

public class DateRange : ValueObject
{
    private DateRange()
    {
    }

    public DateOnly Start { get; private init; }
    public DateOnly End { get; private init; }

    public int LengthInDays => End.DayNumber - Start.DayNumber;

    public static DateRange From(DateOnly start,  DateOnly end)
    {
        if (start > end)
            throw new InvalidOperationException("End date precedes start date");

        return new DateRange
        {
            Start = start,
            End = end
        };
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        
        yield return Start;
        yield return End;
    }
}
