using System.Reflection;
using Castle.Facilities.FactorySupport;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using KServices.Core.Domain.Core.Windsor;
using KServices.Data;
using KServices.Data.Mappings.Mapper;
using KServices.Services;
using NHibernate;

namespace KServices.Locator
{
    public static class WebIoC
    {
        #region Constants and Fields

        /// <summary>
        /// Connection key for accessing to DB.
        /// </summary>
        private const string CONNECTION_KEY = "DBConnectionString";

        /// <summary>
        /// Represents Windsor <c>IoC</c> container.
        /// </summary>
        private static IWindsorContainer _singletonContainer;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the Windsor <c>IoC</c> container.
        /// </summary>
        /// <value>The <c>IoC</c> container.</value>
        public static IWindsorContainer Container
        {
            get
            {
                return _singletonContainer ?? (_singletonContainer = new WindsorContainer().RegisterServices());
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Registers the <c>NHibernate</c> session.
        /// </summary>
        /// <param name="container">The <c>IoC</c> container.</param>
        private static void RegisterNHibernateSession(this IWindsorContainer container)
        {
            container.Register(Component.For<ISessionFactory>().LifeStyle.Singleton.UsingFactoryMethod(() => DbInitializer.Factory));
            container.Register(Component.For<ISession>().LifeStyle.PerWebRequest.UsingFactoryMethod(kernel => kernel.Resolve<ISessionFactory>().OpenSession()));
        }

        /// <summary>
        /// Registers all types.
        /// </summary>
        /// <param name="container">The Windsor <c>IoC</c> container.</param>
        /// <returns>The configured <c>IoC</c> container.</returns>
        private static IWindsorContainer RegisterServices(this IWindsorContainer container)
        {
            var initializer = new DbInitializer(CONNECTION_KEY);
            initializer.Initialize();

            container.RegisterReleasePolicy();
            container.AddFacility<FactorySupportFacility>();

            // Register data access session
            container.RegisterNHibernateSession();

            //// Register all service from email core
            container.RegisterServices(Assembly.GetAssembly(typeof(Authentication)));

            // Register core data access objects
            container.RegisterServices(Assembly.GetAssembly(typeof(DomainMapper)));

            return container;
        }

        #endregion
    }
}
