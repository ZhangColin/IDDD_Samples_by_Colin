using Autofac;
using NUnit.Framework;
using SaasOvation.Common;
using SaasOvation.Common.Domain.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Service;
using SaasOvation.IdentityAccess.Infrastructure;

namespace SaasOvation.IdentityAccess.Domain.Test {
    public abstract class DomainTest: IdentityAccessTest {
        [SetUp]
        protected virtual void SetUp() {

            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<MD5EncryptionService>().As<IEncryptionService>();
            builder.RegisterType<PasswordService>();
            IContainer container = builder.Build();
            ServiceLocator.Resolver = new AutofacResolver(container);

            DomainEventPublisher.Instance.Reset();

        }

        [TearDown]
        protected void TearDown() {
        }
    }
}