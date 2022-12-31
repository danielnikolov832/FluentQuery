namespace FluentQuery.Core;

public interface IQuery<T>
{
    public IReadOnlyList<Predicate<T>> get_rules { get; }
    bool IsValid(T item);
    void Rule(Predicate<T> rule);
    void RuleFor<U>(Func<T, U> partOfT, Predicate<U> rule);
}
