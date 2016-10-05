using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Castle.Windsor;

namespace KServices.Core.Domain.Core.Windsor
{
    public class WindsorDependencyResolver : IDependencyScope
    {
        #region Constants and Fields

        /// <summary>
        ///   The IoC container of Castle.Windsor.
        /// </summary>
        private readonly IWindsorContainer _container;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "WindsorDependencyResolver" /> class.
        /// </summary>
        /// <param name = "container">The IoC container.</param>
        public WindsorDependencyResolver(IWindsorContainer container)
        {
            _container = container;
        }

        #endregion

        #region Public Methods

        public object GetService(Type serviceType)
        {
            return _container.Kernel.HasComponent(serviceType)
                       ? _container.Resolve(serviceType)
                       : null;
        }

        /// <summary>
        ///   Gets the all services.
        /// </summary>
        /// <param name = "serviceType">Type of the service.</param>
        /// <returns>The all resolved objects for service type.</returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            Array servicesAsArray = _container.ResolveAll(serviceType);
            IEnumerable<object> services = servicesAsArray.Cast<object>();

            return services;
        }

        #endregion

        public void Dispose()
        {
            _container.Dispose();
        }
    }
}