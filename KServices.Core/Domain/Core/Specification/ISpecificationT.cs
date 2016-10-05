using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MedTeam.Infrastructure.Specification
{
    /// <summary>
    /// Represents interface for working with query specification where entity is role.<c>http://guildsocial.web703.discountasp.net/dasblogce/2010/02/20/SpecificationPatternAndLinq.aspx</c>
    /// </summary>
    /// <typeparam name="T">The type of entity and role in one.</typeparam>
    public interface ISpecification<T> 
    {
        bool SatisfiedBy(T value);

        IEnumerable<T> SatisfyingElementsFrom(IEnumerable<T> candidates);

        Expression<Func<T, bool>> SatisfiedBy();

        ISpecification<T> And(ISpecification<T> other);

        ISpecification<T> Or(ISpecification<T> other);

        ISpecification<T> Not();
    }    
}