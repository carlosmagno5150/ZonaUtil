using System;
using System.Linq.Expressions;

namespace Util
{   
  public class ReflectionHelper<T> where T : class 
  {
    /// <summary>
    /// Retorna a propriedade por reflection
    /// </summary>
    /// <typeparam name="K">Tipo da propriedade a ser retornado</typeparam>
    /// <param name="obj">Objeto que contem a propriedade</param>
    /// <param name="propName">Propriedade</param>
    /// <returns></returns>
    public K GetProperty<K>(object obj, Expression<Func<T, object>> propName)
    {

      var uExp = (UnaryExpression)propName.Body;
      var mExp = (MemberExpression)uExp.Operand;
      var propriedade = mExp.Member.Name;
      
      return (K)obj.GetType().GetProperty(propriedade).GetValue(obj, null);            
    }


    /// <summary>
    /// Retorna a propriedade por reflection
    /// </summary>
    /// <typeparam name="K">Tipo da propriedade a ser retornado</typeparam>
    /// <param name="obj">Objeto que contem a propriedade</param>    
    /// <param name="propertyName">Nome Propriedade</param>
    /// <returns></returns>
    public K GetProperty<K>(object obj, string propertyName)
    {
      return (K)obj.GetType().GetProperty(propertyName).GetValue(obj, null);
    }
  }
}
