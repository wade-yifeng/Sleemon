using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using Microsoft.Practices.Unity;

namespace Sleemon.WebApi.Factories
{
    public class UnityDependencyResolver : UnityDependencyScope, IDependencyResolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnityDependencyResolver"/> class.
        /// http://www.asp.net/web-api/overview/extensibility/using-the-web-api-dependency-resolver
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        internal UnityDependencyResolver(UnityContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// The begin scope.
        /// http://www.asp.net/web-api/overview/extensibility/using-the-web-api-dependency-resolver
        /// </summary>
        /// <returns>
        /// The <see cref="IDependencyScope"/>.
        /// </returns>
        public IDependencyScope BeginScope()
        {
            return new UnityDependencyScope(this.Container.CreateChildContainer());
        }
    }
}