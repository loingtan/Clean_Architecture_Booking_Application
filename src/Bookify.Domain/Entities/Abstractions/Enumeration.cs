using System.Reflection;

namespace Bookify.Domain.Entities.Abstractions;

public abstract class Enumeration<T> : IComparable where T : Enumeration<T>
{
    public string Name { get; }
    public int Id { get; }

    protected Enumeration(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public override string ToString() => Name;

    public static IEnumerable<T> GetAll() =>
        typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Where(f => f.FieldType == typeof(T))
            .Select(f => f.GetValue(null))
            .Cast<T>();

    public static T FromId(int id)
    {
        return GetAll().FirstOrDefault(e => e.Id == id)
               ?? throw new KeyNotFoundException($"No {typeof(T).Name} found with ID {id}");
    }

    public static T FromName(string name)
    {
        return GetAll().FirstOrDefault(e => e.Name == name)
               ?? throw new KeyNotFoundException($"No {typeof(T).Name} found with Name {name}");
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Enumeration<T> otherValue)
            return false;

        return GetType() == obj.GetType() && Id == otherValue.Id;
    }

    public override int GetHashCode() => Id.GetHashCode();

    public int CompareTo(object? other)
    {
        if (other is null) return 1;
        return Id.CompareTo(((Enumeration<T>)other).Id);
    }
}