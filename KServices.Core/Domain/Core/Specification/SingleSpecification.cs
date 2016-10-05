using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace KServices.Core.Domain.Core.Specification
{
    public class SingleSpecification<T> : Specification<T>        
    {
        #region Constants and Fields

        private readonly Expression<Func<T, bool>> _conditionExpression;

        #endregion

        #region Constructors and Destructors

        public SingleSpecification(Expression<Func<T, bool>> conditionExpression)
        {
            _conditionExpression = conditionExpression;
        }

        #endregion

        #region Public Methods

        public override bool SatisfiedBy(T value)
        {
            return _conditionExpression.Compile().Invoke(value);
        }

        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            return _conditionExpression;
        }

        public override IEnumerable<T> SatisfyingElementsFrom(IEnumerable<T> candidates)
        {
            IEnumerable<T> matchingCandidates = candidates;
            if (_conditionExpression != null)
            {
                matchingCandidates = matchingCandidates is IQueryable<T>
                                         ? ((IQueryable<T>)matchingCandidates).Where(SatisfiedBy())
                                         : matchingCandidates.Where(SatisfiedBy().Compile());
            }

            return matchingCandidates;
        }

        #endregion
    }
}