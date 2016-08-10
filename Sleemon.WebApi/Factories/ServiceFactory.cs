using Microsoft.Practices.Unity;

namespace Sleemon.WebApi.Factories
{
    public class ServiceFactory
    {
        private readonly UnityContainer container;

        public ServiceFactory(UnityContainer container)
        {
            this.container = container;
        }

        public TService GetServiceInstance<TService>()
        {
            return this.container.Resolve<TService>();
        }

        public TService GetServiceInstance<TService>(string name)
        {
            return this.container.Resolve<TService>(name);
        }
    }
}