using System;
using Castle.MicroKernel;
using Castle.Windsor.Diagnostics;

namespace KServices.Core.Domain.Core.Windsor.LifeStileGetAndForgot
{
    /// <summary>
    /// New type of Lifestyle. Create instance and forget it. It clean by .net garbage cleaner.
    /// </summary>
    [Serializable]
    public class LifecycledComponentsReleasePolicy : Castle.MicroKernel.Releasers.LifecycledComponentsReleasePolicy
    {
        #region Constants and Fields

        /// <summary>
        /// Type of the inherits class InstantiateAndForgetIt
        /// </summary>
        private readonly Type _instantiateAndForgetItType = typeof(InstantiateAndForgetIt);

        #endregion

        #region Public Methods

        public LifecycledComponentsReleasePolicy(IKernel kernel)
            : base(kernel)
        {
        }

        public LifecycledComponentsReleasePolicy(ITrackedComponentsDiagnostic trackedComponentsDiagnostic, ITrackedComponentsPerformanceCounter trackedComponentsPerformanceCounter)
            : base(trackedComponentsDiagnostic, trackedComponentsPerformanceCounter)
        {
        }

        /// <summary>
        /// Tracks the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="burden">The burden.</param>
        public override void Track(object instance, Burden burden)
        {
            if (_instantiateAndForgetItType.Equals(burden.Model.CustomLifestyle))
            {
                return;
            }

            base.Track(instance, burden);
        }

        #endregion
    }
}