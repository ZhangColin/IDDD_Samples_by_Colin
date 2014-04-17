using System;
using System.Collections;
using System.Collections.Generic;
using Autofac;

namespace SaasOvation.Common {
    public class AutofacResolver: IResolver {
        private readonly IContainer _container;

        public AutofacResolver(IContainer container) {
            this._container = container;
        }

        public object GetService(Type serviceType) {
            return _container.Resolve(serviceType);
        }

        public TService GetService<TService>() {
            return _container.Resolve<TService>();
        }

        public IEnumerable GetServices(Type serviceType) {
            throw new NotImplementedException();
        }

        public IEnumerable<TService> GetServices<TService>() {
            throw new NotImplementedException();
        }
    }
}