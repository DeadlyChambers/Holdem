using Ninject;
using System;
using System.Collections.Generic;
using Holdem.Services;


namespace MCV.Skeleton.Infrastructure
{
    public class NinjectDependencyResolver : System.Web.Mvc.IDependencyResolver, System.Web.Http.Dependencies.IDependencyResolver
    {
        public NinjectDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;

            AddBindings();
        }



        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }



        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }



        private void AddBindings()
        {
            _kernel.Bind<ITableService>().To<TableService>();
        }



        public System.Web.Http.Dependencies.IDependencyScope BeginScope()
        {
            return this;
        }



        public void Dispose()
        {
            // When BeginScope returns 'this', the Dispose method must be a no-op.
        }




        private IKernel _kernel;
    }
}

