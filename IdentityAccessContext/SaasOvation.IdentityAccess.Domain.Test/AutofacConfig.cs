using Autofac;
using SaasOvation.Common;
using SaasOvation.IdentityAccess.Domain.Access.Service;
using SaasOvation.IdentityAccess.Domain.Identity.Service;
using SaasOvation.IdentityAccess.Infrastructure;

namespace SaasOvation.IdentityAccess.Domain.Test {
    public static class AutofacConfig {

        private static void RegisterService(ContainerBuilder builder) {
            builder.RegisterType<GroupMemberService>().InstancePerLifetimeScope();
            builder.RegisterType<AuthenticationService>().InstancePerLifetimeScope();
            builder.RegisterType<AuthorizationService>().InstancePerLifetimeScope();
            builder.RegisterType<PasswordService>().InstancePerLifetimeScope();
            builder.RegisterType<MD5EncryptionService>().As<IEncryptionService>().InstancePerLifetimeScope();
            builder.RegisterType<TenantProvisioningService>().InstancePerLifetimeScope();
        }

        public static void InitializeForNH() {
            ContainerBuilder builder = new ContainerBuilder();

            RegisterService(builder);
            RegisterNhRepository(builder);

            IContainer container = builder.Build();

            ServiceLocator.Resolver = new AutofacResolver(container);
        }

        private static void RegisterNhRepository(ContainerBuilder builder) {
//            builder.RegisterType<InMemoryGroupRepository>().As<IGroupRepository>().InstancePerLifetimeScope();
//            builder.RegisterType<InMemoryRoleRepository>().As<IRoleRepository>().InstancePerLifetimeScope();
//            builder.RegisterType<InMemoryTenantRepository>().As<ITenantRepository>().InstancePerLifetimeScope();
//            builder.RegisterType<InMemoryUserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
        }
    }
}