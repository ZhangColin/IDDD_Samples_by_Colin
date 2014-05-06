using NUnit.Framework;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Collaboration.Domain.Test {
    public abstract class DomainTest {
        [SetUp]
        protected virtual void SetUp() {

//            ContainerBuilder builder = new ContainerBuilder();
//            IContainer container = builder.Build();
//            ServiceLocator.Resolver = new AutofacResolver(container);

            DomainEventPublisher.Instance.Reset();

        }

        [TearDown]
        protected void TearDown() {
        }
    }
}