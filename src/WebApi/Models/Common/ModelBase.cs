namespace CM.WebApi.Models.Common;

public abstract class ModelBase : ModelBaseGeneric<long>
{
}

public abstract class ModelBaseGeneric<T> where T : struct
{
    public T Id { get; set; }
}
