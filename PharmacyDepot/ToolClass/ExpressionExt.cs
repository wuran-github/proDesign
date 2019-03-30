using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
namespace PharmacyDepot.ToolClass
{
    /// <summary>
    /// 扩展方法
    /// 以及lambda表达式的扩展应用
    /// </summary>
    public static class ExpressionExt
    {
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> exp1, Expression<Func<T, bool>> exp2)
        {
            return Compose(exp1, exp2, Expression.AndAlso);
            //return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(exp1.Body, exp2.Body), exp1.Parameters);
        }
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> exp1, Expression<Func<T, bool>> exp2)
        {
            return Compose(exp1, exp2, Expression.OrElse);
            //return Expression.Lambda<Func<T, bool>>(Expression.OrElse(exp1.Body, exp2.Body), exp1.Parameters);
        }
        private static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // build parameter map (from parameters of second to parameters of first)
            //   selector:
            //     一个应用于每个源元素的转换函数；函数的第二个参数表示源元素的索引。
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);
            first.Parameters.Select((f, i) => new { });
            // replace parameters in the second lambda expression with parameters from the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // apply composition of lambda expression bodies to parameters from the first expression 
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }
    }
    partial class ParameterRebinder : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> map;

        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }
        //
        // 摘要: 
        //     访问 System.Linq.Expressions.ParameterExpression。
        //
        // 参数: 
        //   node:
        //     要访问的表达式。
        //
        // 返回结果: 
        //     如果修改了该表达式或任何子表达式，则为修改后的表达式；否则返回原始表达式。
        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression replacement;
            if (map.TryGetValue(p, out replacement))
            {
                p = replacement;
            }
            return base.VisitParameter(p);
        }
    }
}