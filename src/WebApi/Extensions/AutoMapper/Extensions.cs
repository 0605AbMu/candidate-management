using AutoMapper;
using CM.WebApi.Models.Common;

namespace CM.WebApi.Extensions.AutoMapper;

public static class Extensions
{
    public static IMappingExpression<TSource, TDestination> IgnoreModelBase<TSource, TDestination>(
        this IMappingExpression<TSource, TDestination> expression) where TSource : ModelBase
        where TDestination : ModelBase
    {
        expression.ForMember(x => x.Id, x => x.Ignore());
        return expression;
    }

    public static IMappingExpression<TSource, TDestination> IgnoreAuditable<TSource, TDestination>(
        this IMappingExpression<TSource, TDestination> expression) where TSource : AuditableModelBase
        where TDestination : AuditableModelBase
    {
        expression
            .ForMember(x => x.Id, x => x.Ignore())
            .ForMember(x => x.CreatedAt, x => x.Ignore())
            .ForMember(x => x.UpdatedAt, x => x.Ignore());

        return expression;
    }
}
