using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using Bookify.Application.Common.Interfaces;
using Bookify.Application.Common.Models;

namespace Bookify.Infrastructure.DataShaping;

public class DataShaper<T> : IDataShaper<T>
{
    private readonly PropertyInfo[] _properties;

    public DataShaper()
    {
        _properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
    }

    public IEnumerable<ShapedData> ShapeData(IEnumerable<T> entities, string fieldsString)
    {
        var requiredProperties = GetRequiredProperties(fieldsString);

        return FetchData(entities, requiredProperties);
    }

    public ShapedData ShapeData(T entity, string fieldsString)
    {
        var requiredProperties = GetRequiredProperties(fieldsString);

        return FetchDataForEntity(entity, requiredProperties);
    }

    private IEnumerable<PropertyInfo> GetRequiredProperties(string fieldsString)
    {
        var requiredProperties = new List<PropertyInfo>();

        if (string.IsNullOrWhiteSpace(fieldsString))
        {
            requiredProperties = _properties.ToList();
            return requiredProperties;
        }

        var fields = fieldsString.Split(',', StringSplitOptions.RemoveEmptyEntries);

        foreach (var field in fields)
        {
            var property = _properties.FirstOrDefault(
                pi => pi.Name.Equals(field.Trim(), StringComparison.InvariantCultureIgnoreCase));

            if (property != null)
            {
                requiredProperties.Add(property);
            }
        }

        return requiredProperties;
    }

    private IEnumerable<ShapedData> FetchData(IEnumerable<T> entities, IEnumerable<PropertyInfo> requiredProperties)
    {
        var shapedData = new List<ShapedData>();

        foreach (var entity in entities)
        {
            var shapedObject = FetchDataForEntity(entity, requiredProperties);
            shapedData.Add(shapedObject);
        }

        return shapedData;
    }

    private ShapedData FetchDataForEntity(T entity, IEnumerable<PropertyInfo> requiredProperties)
    {
        var shapedObject = new ExpandoObject();

        foreach (var property in requiredProperties)
        {
            var objectPropertyValue = property.GetValue(entity);
            ((IDictionary<string, object>)shapedObject).Add(property.Name, objectPropertyValue);
        }

        return new ShapedData(shapedObject);
    }
}