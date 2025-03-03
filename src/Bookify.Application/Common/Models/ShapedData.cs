using System.Collections.Generic;

namespace Bookify.Application.Common.Models;

public class ShapedData
{
    public object Value { get; set; }

    public ShapedData(object value)
    {
        Value = value;
    }
}