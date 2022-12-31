using System.Collections;
using System.Linq.Expressions;

namespace FluentQuery.Core;

public static class QueryingExtensions
{
    public static List<T> Query<T>(this IEnumerable<T> values, IQuery<T> query)
    {
        List<T> output = new();

        output.AddRange(from T item in values
                        where query.IsValid(item)
                        select item);

        return output;
    }

    public static List<T> Query<T>(this IQueryable<T> values, IQuery<T> query)
    {
        List<T> output = new();

        output.AddRange(from T item in values
                        where query.IsValid(item)
                        select item);

        return output;
    }

    public static IQueryable<T> QueryAndReturnQueryable<T>(this IEnumerable<T> values, IQuery<T> query)
    {
        List<T> output = new();

        output.AddRange(from T item in values
                        where query.IsValid(item)
                        select item);

        return new EnumerableQuery<T>(output);
    }

    public static IQueryable<T> QueryAndReturnQueryable<T>(this IQueryable<T> values, IQuery<T> query)
    {
        List<T> output = new();

        output.AddRange(from T item in values
                        where query.IsValid(item)
                        select item);

        return new EnumerableQuery<T>(output);
    }
}