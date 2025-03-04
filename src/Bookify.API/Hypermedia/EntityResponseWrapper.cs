namespace Bookify.API.Hypermedia;

public class EntityResponseWrapper<T> : LinkedResourceBase where T : class
{
    public T Value { get; set; }

    public EntityResponseWrapper(T value)
    {
        Value = value;
    }
}