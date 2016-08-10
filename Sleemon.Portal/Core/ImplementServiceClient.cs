using Microsoft.Practices.Unity;
using Sleemon.Portal.Common;
using Sleemon.Portal.Factories;
using System;

namespace Sleemon.Portal.Core
{
    public class ImplementServiceClient
    {
        private readonly ServiceFactory serviceFactory;

        public ImplementServiceClient(
            [Dependency]ServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
        }

        public TResult Request<TService, TResult>(Func<TService, TResult> action)
        {
            try
            {
                var service =
                        serviceFactory.GetServiceInstance<TService>();
                return action(service);
            }
            catch (Exception ex)
            {
                LogHelper<ImplementServiceClient>.WriteException(ex);
                return default(TResult);
            }
        }
    }
}