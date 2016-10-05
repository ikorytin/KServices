using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate.Hql.Ast;
using NHibernate.Linq;
using NHibernate.Linq.Functions;
using NHibernate.Linq.Visitors;

namespace KServices.Data.HqlGenerators
{
    public class ConcatHqlGenerator : BaseHqlGeneratorForMethod
    {
        public ConcatHqlGenerator()
        {
            SupportedMethods = new[] { ReflectionHelper.GetMethodDefinition(() => string.Concat(null, null)) };
        }

        public override HqlTreeNode BuildHql(MethodInfo method, Expression targetObject, ReadOnlyCollection<Expression> arguments, HqlTreeBuilder treeBuilder, IHqlExpressionVisitor visitor)
        {
            return treeBuilder.Concat(
                new[] 
            {
                visitor.Visit(arguments[0]).AsExpression(), 
                visitor.Visit(arguments[1]).AsExpression()
            });
        }
    }
}