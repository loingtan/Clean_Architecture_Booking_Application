using System.Collections.Generic;
using Bookify.Application.Common.Models;

namespace Bookify.Application.Common.Interfaces;

public interface IDataShaper<T>
{
    IEnumerable<ShapedData> ShapeData(IEnumerable<T> entities, string fieldsString);
    ShapedData ShapeData(T entity, string fieldsString);
}