using System;
using System.Linq.Expressions;

namespace SpecificationPattern
{
  public abstract class Specification<T>
  {
    private readonly ParameterExpression _pe;
    private Expression _exp;

    protected Specification()
    {
      _pe = Expression.Parameter(typeof(T), "t");
      var left = Expression.Constant(1);
      var right = Expression.Constant(1);
      _exp = Expression.Equal(left, right);
    }    

    public Specification<T> And(Expression<Predicate<T>> func)
    {
      _exp = Expression.And(Expression.Invoke(func, _pe), _exp);
      return this;
    }    

    public Specification<T> AndAlso(Expression<Predicate<T>> func)
    {
      _exp = Expression.AndAlso(Expression.Invoke(func, _pe), _exp);
      return this;
    }

    public Specification<T> Or(Expression<Predicate<T>> func)
    {
      _exp = Expression.Or(Expression.Invoke(func, _pe), _exp);
      return this;
    }

    public Specification<T> OrElse(Expression<Predicate<T>> func)
    {
      _exp = Expression.OrElse(Expression.Invoke(func, _pe), _exp);
      return this;
    }

    public Func<T, bool> GetQuery()
    {
      return Expression.Lambda<Func<T, bool>>(_exp, _pe).Compile();
    }

  }
}
