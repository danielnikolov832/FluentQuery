using FluentQuery.Core;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace FluentQuery.SQLSupport;

public static class QueryToExpression
{
    public static Expression<Func<T, bool>> QueryToExp<T>(QueryForSQLBase<T> query)
    {
        var start = PredicateBuilder.New<T>(true);

        foreach (Expression<Func<T, bool>> rule in query.get_rules)
        {
            start = start.And(rule);
        }

        return start;
    }

    public static Expression<Func<T, bool>> QueryToExp<T>(IQuery<T> query)
    {
        var start = PredicateBuilder.New<T>(true);

        foreach (Predicate<T> rule in query.get_rules)
        {
            start = start.And(x => rule(x));
        }

        return start;
    }
}

#pragma warning disable S101 // Types should be named in PascalCase
public class QueryForSQLBase<T>
#pragma warning restore S101 // Types should be named in PascalCase
{
    private readonly List<Expression<Func<T, bool>>> _rules;

    public IReadOnlyList<Expression<Func<T, bool>>> get_rules => _rules;

    public QueryForSQLBase()
    {
        _rules = new();
    }

    public void Rule(Expression<Func<T, bool>> rule)
    {
        _rules.Add(rule);
    }

    public void RuleFor<U>(Func<T, U> partOfT, Predicate<U> rule)
    {
        _rules.Add((T item) => rule(partOfT(item)));
    }

    public bool IsValid(T item)
    {
        foreach (Expression<Func<T, bool>> rule in _rules)
        {
            if (!rule.Invoke(item)) return false;
        }

        return true;
    }

    public static implicit operator Expression<Func<T, bool>>(QueryForSQLBase<T> query)
    {
        return t => query.IsValid(t);
    }
}