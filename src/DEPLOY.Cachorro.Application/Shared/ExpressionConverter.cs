using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using DEPLOY.Cachorro.Application.Dtos;

namespace DEPLOY.Cachorro.Application.Shared
{
    [ExcludeFromCodeCoverage]
    public class ExpressionConverter : ExpressionVisitor
    {
        private readonly ParameterExpression _parameter;

        public ExpressionConverter(ParameterExpression parameter)
        {
            _parameter = parameter;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return _parameter;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if(node.Member.DeclaringType == typeof(CachorroDto))
            {
                return Expression.Property(Visit(node.Expression), node.Member.Name);
            }

            return base.VisitMember(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if(node.NodeType == ExpressionType.Equal || node.NodeType == ExpressionType.NotEqual) 
            {
                if(node.Left.NodeType == ExpressionType.Constant && ((ConstantExpression)node.Left).Value == null)
                {
                    return Expression.MakeBinary(node.NodeType, Visit(node.Right), node.Left);
                } 
                else if (node.Right.NodeType == ExpressionType.Constant && ((ConstantExpression)node.Right).Value == null)
                {
                    return Expression.MakeBinary(node.NodeType, Visit(node.Left), node.Right);
                }
            }

            return base.VisitBinary(node);
        }
    }
}