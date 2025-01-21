using Bookify.Domain.Entities.Abstractions;

namespace Bookify.Domain.Entities.Reviews.ValueObjects;

public class  Rating: ValueObject
{
    public static readonly Error Invalid = new("Rating.Invalid", "The rating is invalid");

    private Rating(int value) => Value = value;

    public int Value { get; init; }

    public static Result<Rating> Create(int value)
    {
        if (value < 1 || value > 5)
            return Result.Failure<Rating>(Invalid);

        return new Rating(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
       
        yield return Value;
    }
}
