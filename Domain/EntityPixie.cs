using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MassTransit;

namespace Elfland.Lake.Domain;

public abstract class EntityPixie
{
    [Key]
    public virtual Guid? Id { get; protected set; } = NewId.NextGuid();

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime? CreatedTime { get; init; } = DateTime.UtcNow;

    public string? Remark { get; set; }

    private int? _requestedHashCode;

    public bool IsTransient() => Id == default;

    public static bool operator ==(EntityPixie left, EntityPixie right) =>
        Equals(left, null) ? Equals(right, null) : left.Equals(right);

    public static bool operator !=(EntityPixie left, EntityPixie right) => !(left == right);

    public override bool Equals(object? obj)
    {
        if (obj is null || obj is not EntityPixie)
            return false;

        if (Object.ReferenceEquals(this, obj))
            return true;

        if (this.GetType() != obj.GetType())
            return false;

        EntityPixie item = (EntityPixie)obj;

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
