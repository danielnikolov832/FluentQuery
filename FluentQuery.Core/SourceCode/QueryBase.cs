using System.Linq;
using System.Linq.Expressions;

namespace FluentQuery.Core;

public class QueryBase<T> : IQuery<T>
{
    private readonly List<Predicate<T>> _rules;

    public IReadOnlyList<Predicate<T>> get_rules => _rules;

    public QueryBase()
    {
        _rules = new();
    }

    public QueryBase(params Predicate<T>[] rules)
    {
        _rules = rules.ToList();
    }

    public void Rule(Predicate<T> rule)
    {
        _rules.Add(rule);
    }

    public void RuleFor<U>(Func<T, U> partOfT, Predicate<U> rule)
    {
        _rules.Add((T item) => rule(partOfT(item)));
    }

    public bool IsValid(T item)
    {
        foreach (Predicate<T> rule in _rules)
        {
            if (!rule(item)) return false;
        }

        return true;
    }

    public static List<T> Query(IEnumerable<T> values, IQuery<T> query)
    {
        return values.Query(query);
    }

    public List<T> Query(IEnumerable<T> values)
    {
        return values.Query(this);
    }

    public static implicit operator Expression<Func<List<T>, List<T>>>(QueryBase<T> query)
    {
        return t => Query(t, query);
    }

    public static implicit operator Expression<Func<T, bool>>(QueryBase<T> query)
    {
        return t => query.IsValid(t);
    }
}