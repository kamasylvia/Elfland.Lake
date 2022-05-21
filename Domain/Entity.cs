namespace Elfland.Lake.Domain;

public abstract class Entity
{
    private int? _requestedHashCode;
    public virtual Guid? Id { get; protected set; }

    public bool IsTransient() => Id == default;

    public static bool operator ==(Entity left, Entity right) =>
        Equals(left, null) ? Equals(right, null) : left.Equals(right);

    public static bool operator !=(Entity left, Entity right) => !(left == right);

    public override bool Equals(object? obj)
    {
        if (obj is null || obj is not Entity)
            return false;

        if (Object.ReferenceEquals(this, obj))
            return true;

        if (this.GetType() != obj.GetType())
            return false;

        Entity item = (Entity)obj;

        return item.IsTransient() || IsTransient() ? false : item.Id == this.Id;
    }

    public override int GetHashCode()
    {
        if (!IsTransient())
        {
            if (!_requestedHashCode.HasValue)
                _requestedHashCode = Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

            return _requestedHashCode.Value;
        }
        else
            return base.GetHashCode();
    }
}
