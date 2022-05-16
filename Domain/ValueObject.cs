namespace Elfland.Lake.Domain;

public abstract class ValueObject
{
    protected static bool EqualOperator(ValueObject left, ValueObject right) =>
        !(left is null ^ right is null) && (left is null || left.Equals(right!));

    protected static bool NotEqualOperator(ValueObject left, ValueObject right) =>
        !EqualOperator(left, right);

    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = obj as ValueObject;

        return GetEqualityComponents().SequenceEqual(other!.GetEqualityComponents());
    }

    public override int GetHashCode() =>
        GetEqualityComponents()
            .Select(x => x is not null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);

    public ValueObject? GetCopy() => MemberwiseClone() as ValueObject;
}
