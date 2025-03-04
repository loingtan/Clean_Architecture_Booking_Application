using System.Collections.Generic;

namespace Bookify.API.Hypermedia;

public class CollectionResponseWrapper<T> : LinkedResourceBase where T : class
{
    public IEnumerable<T> Value { get; set; }
    public Pagination Pagination { get; set; }

    public CollectionResponseWrapper(IEnumerable<T> value, Pagination pagination = null)
    {
        Value = value;
        Pagination = pagination;
    }
}