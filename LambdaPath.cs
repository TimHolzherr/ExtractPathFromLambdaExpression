using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FuncToString;

public static class LambdaPath<TSource>
{
    public static string Get<TResult>(Expression<Func<TSource, TResult>> expression)
    {
        var visitor = new PathPrintVisitor();
        visitor.Visit(expression.Body);
        visitor.Path.Reverse();
        return string.Join("", visitor.Path)
            .Substring(1);
    }

    private class PathPrintVisitor : ExpressionVisitor
    {
        internal readonly List<string> Path = new();

        protected override Expression VisitMember(MemberExpression node)
        {
            if (!(node.Member is PropertyInfo))
            {
                throw new ArgumentException("The path can only contain properties", nameof(node));
            }

            Path.Add($".{node.Member.Name}");
            return base.VisitMember(node);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.Name == "get_Item") // index access for IList
            {
                var argument = node.Arguments.FirstOrDefault();
                if (argument != null)
                {
                    HandleIndexes(argument);
                }
            }

            return base.VisitMethodCall(node);
        }

        private void HandleIndexes(Expression argument)
        {
            if (argument.NodeType == ExpressionType.Constant && argument.Type == typeof(int))
            {
                int value = (int)((ConstantExpression)argument).Value;
                Path.Add(value is 42 or -1 ? "[*]" : $"[{value}]");
            }
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.NodeType == ExpressionType.ArrayIndex &&
                node.Right.NodeType == ExpressionType.Constant)
            {
                HandleIndexes(node.Right);
            }

            return base.VisitBinary(node);
        }
    }
}