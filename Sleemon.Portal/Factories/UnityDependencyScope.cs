using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

namespace Sleemon.Portal.Factories
{
    public class UnityDependencyScope : IDependencyScope
    {
        protected readonly IUnityContainer Container;

        internal UnityDependencyScope([Dependency]IUnityContainer container)
        {
            this.Container = container;
        }

        /// <summary>
        /// The get service.
        /// http://www.asp.net/web-api/overview/extensibility/using-the-web-api-dependency-resolver
        /// </summary>
        /// <param name="serviceType">
        /// The service type.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public object GetService(Type serviceType)
        {
            if (serviceType != null)
            {
                try
                {
                    return this.Container.Resolve(serviceType);
                }
                catch (ResolutionFailedException)
                {
                }
            }

            return null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (serviceType != null)
            {
                try
                {
                    return this.Container.ResolveAll(serviceType);
                }
                catch (ResolutionFailedException)
                {
                }
            }

            return new List<object>();
        }

        /// <summary>
        /// The dispose.
        /// http://www.asp.net/web-api/overview/extensibility/using-the-web-api-dependency-resolver
        /// </summary>
        public void Dispose()
        {
            this.Container.Dispose();
        }
    }
}