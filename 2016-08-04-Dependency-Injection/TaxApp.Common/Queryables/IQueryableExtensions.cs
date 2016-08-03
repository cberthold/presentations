using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace System.Linq
{
    public static class IQueryableExtensions
    {
        public static Expression<Func<T, bool>> True<T>()
        {
            Expression<Func<T, bool>> expr = a => true;
            return expr;
        }

        public static Expression<Func<T, bool>> False<T>()
        {
            Expression<Func<T, bool>> expr = a => false;
            return expr;
        }

        /// <summary>
        /// Handles joining two linq expressions using AND
        /// </summary>
        /// <typeparam name="T">Type the expression is returning boolean on</typeparam>
        /// <param name="leftExpression">input expression</param>
        /// <param name="rightExpression">expression being joined by And</param>
        /// <returns>expresion representing And join of both expressions</returns>  
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> leftExpression, Expression<Func<T, bool>> rightExpression)
        {

            ParameterExpression p = leftExpression.Parameters[0];

            SubstExpressionVisitor visitor = new SubstExpressionVisitor();
            visitor.subst[rightExpression.Parameters[0]] = p;

            Expression body = Expression.AndAlso(leftExpression.Body, visitor.Visit(rightExpression.Body));
            return Expression.Lambda<Func<T, bool>>(body, p);
        }

        /// <summary>
        /// Handles joining two linq expressions using OR
        /// </summary>
        /// <typeparam name="T">Type the expression is returning boolean on</typeparam>
        /// <param name="leftExpression">input expression</param>
        /// <param name="rightExpression">expression being joined by OR</param>
        /// <returns>expresion representing OR join of both expressions</returns>        
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> leftExpression, Expression<Func<T, bool>> rightExpression)
        {

            ParameterExpression p = leftExpression.Parameters[0];

            SubstExpressionVisitor visitor = new SubstExpressionVisitor();
            visitor.subst[rightExpression.Parameters[0]] = p;

            Expression body = Expression.OrElse(leftExpression.Body, visitor.Visit(rightExpression.Body));
            return Expression.Lambda<Func<T, bool>>(body, p);
        }


        /// <summary>
        /// Handles visiting for the expression to do parameter substitution
        /// </summary>
        internal class SubstExpressionVisitor : ExpressionVisitor
        {
            public Dictionary<Expression, Expression> subst = new Dictionary<Expression, Expression>();

            protected override Expression VisitParameter(ParameterExpression node)
            {
                Expression newValue;
                if (subst.TryGetValue(node, out newValue))
                {
                    return newValue;
                }
                return node;
            }
        }
    }
}
