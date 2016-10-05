using System.Collections.Generic;
using System.Linq.Expressions;

namespace MedTeam.Infrastructure.Specification
{
    public class ParameterRebinder : ExpressionVisitor
    {
        #region Constants and Fields

        private readonly Dictionary<ParameterExpression, ParameterExpression> _map;

        #endregion

        #region Constructors and Destructors

        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            _map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        #endregion

        #region Public Methods

        public static Expression ReplaceParameters(
            Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }

        #endregion

        #region Methods

        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression replacement;
            if (_map.TryGetValue(p, out replacement))
            {
                p = replacement;
            }

            return base.VisitParameter(p);
        }

        #endregion
    }
}