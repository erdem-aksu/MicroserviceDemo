using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MicroserviceDemo.Web.Helpers
{
    public static class PropertyPath<TSource>
    {
        public static IReadOnlyList<MemberInfo> Get<TResult>(Expression<Func<TSource, TResult>> expression)
        {
            var visitor = new PropertyVisitor();

            visitor.Visit(expression.Body);

            visitor.Path.Reverse();

            return visitor.Path;
        }

        public static string GetString<TResult>(Expression<Func<TSource, TResult>> expression)
        {
            return Get(expression).Select(m => m.Name).JoinAsString(".");
        }

        private class PropertyVisitor : ExpressionVisitor
        {
            internal readonly List<MemberInfo> Path = new List<MemberInfo>();

            protected override Expression VisitMember(MemberExpression node)
            {
                if (node.Member is not PropertyInfo)
                {
                    throw new ArgumentException("The path can only contain properties", nameof(node));
                }

                Path.Add(node.Member);

                return base.VisitMember(node);
            }
        }
    }
}