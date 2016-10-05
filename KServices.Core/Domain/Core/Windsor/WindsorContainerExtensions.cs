using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Diagnostics;
using KServices.Core.Domain.Core.Windsor.LifeStileGetAndForgot;
using LifecycledComponentsReleasePolicy = Castle.MicroKernel.Releasers.LifecycledComponentsReleasePolicy;

namespace KServices.Core.Domain.Core.Windsor
{
    public static class WindsorContainerExtensions
    {
        #region Public Methods

        /// <summary>
        /// Registers the all services from assembly.
        /// </summary>
        /// <param name="container">The <c>IoC</c> container.</param>
        /// <param name="assembly">The assembly with services.</param>
        public static void RegisterServices(this IWindsorContainer container, Assembly assembly)
        {
            container.Register(
                Classes.FromAssembly(assembly).Where(t => true).WithService.AllInterfaces().Configure(
                    reg => reg.LifeStyle.Custom<InstantiateAndForgetIt>()));
        }

        public static IWindsorContainer RegisterReleasePolicy(this IWindsorContainer container)
        {
            ITrackedComponentsDiagnostic diagnostic = LifecycledComponentsReleasePolicy.GetTrackedComponentsDiagnostic(container.Kernel);
            ITrackedComponentsPerformanceCounter counter = LifecycledComponentsReleasePolicy.GetTrackedComponentsPerformanceCounter(new PerformanceMetricsFactory());
            container.Kernel.ReleasePolicy = new LifecycledComponentsReleasePolicy(diagnostic, counter);
            return container;
        }

        #endregion 
    }
}