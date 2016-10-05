using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MedTeam.Infrastructure.Specification;

namespace KServices.Core.Domain.Core.Specification.Binary
{
    public abstract class BinarySpecification<T> : Specification<T>
    {
        #region Constructors and Destructors

        protected BinarySpecification(ISpecification<T> left, ISpecification<T> right)
        {
            LeftSide = left;
            RightSide = right;
        }

        #endregion

        #region Properties

        protected ISpecification<T> LeftSide { get; private set; }

        protected ISpecification<T> RightSide { get; private set; }

        #endregion

        #region Public Methods

        public abstract override bool SatisfiedBy(T value);

        public abstract override Expression<Func<T, bool>> SatisfiedBy();

        /// <summary>
        ///   Satisfying the elements from candidates.
        /// </summary>
        /// <param name="candidates"> The candidates. </param>
        /// <returns> The candidates which satisfied by specification. </returns>
        public override IEnumerable<T> SatisfyingElementsFrom(IEnumerable<T> candidates)
        {
            IEnumerable<T> matchingCandidates = candidates;
            if (LeftSide != null && RightSide != null)
            {
                matchingCandidates = matchingCandidates is IQueryable<T>
                                         ? ((IQueryable<T>)matchingCandidates).Where(SatisfiedBy())
                                         : matchingCandidates.Where(SatisfiedBy().Compile());
            }

            return matchingCandidates;
        }

        #endregion

        #region Methods

        protected Expression<Func<T, bool>> Compose(Expression<Func<T, bool>> first, Expression<Func<T, bool>> second, Func<Expression, Expression, Expression> merge)
        {
            // build parameter map (from parameters of second to parameters of first)
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with parameters from the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // apply composition of lambda expression bodies to parameters from the first expression             
            return Expression.Lambda<Func<T, bool>>(merge(first.Body, secondBody), first.Parameters);
        }

        #endregion
    }
}