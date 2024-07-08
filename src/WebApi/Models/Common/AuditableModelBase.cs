namespace CM.WebApi.Models.Common;

public class AuditableModelBase : AuditableModelBaseGeneric<long>
{
}

public class AuditableModelBaseGeneric<T> : ModelBaseGeneric<T> where T : struct
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
