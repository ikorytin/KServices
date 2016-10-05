using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MedTeam.Infrastructure.Specification;

namespace KServices.Core.Domain.Core.Specification.Unary
{
    public class NotSpecification<T> : Specification<T>
    {
        protected ISpecification<T> Negate { get; private set; }

        public NotSpecification(ISpecification<T> negate)
        {
            this.Negate = negate;
        }

        public override bool SatisfiedBy(T value)
        {
            return !Negate.SatisfiedBy(value);
        }

        public override IEnumerable<T> SatisfyingElementsFrom(IEnumerable<T> candidates)
        {
            var matchingCandidates = candidates;
            if (SatisfiedBy() != null)
            {
                matchingCandidates = matchingCandidates is IQueryable<T>
                                         ? ((IQueryable<T>)matchingCandidates).Where(SatisfiedBy())
                                         : matchingCandidates.Where(SatisfiedBy().Compile());
            }

            IEnumerable<T> result = matchingCandidates.ToList().AsQueryable();

            return result;
        }

        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            var body = Expression.Not(Negate.SatisfiedBy().Body);
            var lambda = Expression.Lambda<Func<T, bool>>(body, Negate.SatisfiedBy().Parameters);

            return lambda;
        }
    }
}