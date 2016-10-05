using System;
using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;

namespace KServices.Core.Domain.Core.Windsor.LifeStileGetAndForgot
{
    /// <summary>
    /// IoC Life Style manager
    /// </summary>
    [Serializable]
    public class InstantiateAndForgetIt : ILifestyleManager
    {
        #region Constants and Fields

        private IComponentActivator _componentActivator;
        private IKernel _kernel;
        private ComponentModel _model;

        protected IComponentActivator ComponentActivator
        {
            get { return _componentActivator; }
        }

        protected IKernel Kernel
        {
            get { return _kernel; }
        }

        protected ComponentModel Model
        {
            get { return _model; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        public virtual void Init(IComponentActivator componentActivator, IKernel kernel, ComponentModel model)
        {
            this._componentActivator = componentActivator;
            this._kernel = kernel;
            this._model = model;
        }

        /// <summary>
        /// Releases the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>Always return true</returns>
        public bool Release(object instance)
        {
            return true;
        }

        public object Resolve(CreationContext context, IReleasePolicy releasePolicy)
        {
            var burden = CreateInstance(context, false);
            Track(burden, releasePolicy);
            return burden.Instance;
        }

        protected virtual Burden CreateInstance(CreationContext context, bool trackedExternally)
        {
            var burden = context.CreateBurden(ComponentActivator, trackedExternally);

            var instance = _componentActivator.Create(context, burden);
            return burden;
        }

        protected virtual void Track(Burden burden, IReleasePolicy releasePolicy)
        {
            if (burden.RequiresPolicyRelease)
            {
                releasePolicy.Track(burden.Instance, burden);
            }
        }

        #endregion
    }
}