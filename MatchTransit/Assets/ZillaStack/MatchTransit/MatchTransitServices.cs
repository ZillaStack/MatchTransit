using Microsoft.MinIoC;
using System;

namespace Assets.ZillaStack.MatchTransit
{
    public static class MatchTransitServices
    {
        private static Container _container;

        public static void ConfigureServices(IServiceProvider serviceProvider = null)
        {
            if (_container != null)
            {
                throw new InvalidOperationException("MatchTransitServices is already configured, the ConfigureServices method should only be called once on application startup.");
            }

            if (serviceProvider == null)
            {
                serviceProvider = new Container();
            }

            _container = (Container)serviceProvider;

            //_container.Register<IFoo>(typeof(Foo));
        }

        public static T Get<T>() => _container.Resolve<T>();
    }
}
