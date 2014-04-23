using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using SaasOvation.Common;
using SaasOvation.IdentityAccess.Application;
using SaasOvation.IdentityAccess.Domain.Access.Repository;
using SaasOvation.IdentityAccess.Domain.Identity.Repository;
using SaasOvation.IdentityAccess.Domain.Identity.Service;
using SaasOvation.IdentityAccess.Infrastructure;
using SaasOvation.IdentityAccess.Infrastructure.Persistence;

namespace SaasOvation.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<TenantRepository>().As<ITenantRepository>();
            builder.RegisterType<GroupRepository>().As<IGroupRepository>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<RoleRepository>().As<IRoleRepository>();

            builder.RegisterType<MD5EncryptionService>().As<IEncryptionService>();

            builder.RegisterType<AuthenticationService>();
            builder.RegisterType<GroupMemberService>();
            builder.RegisterType<PasswordService>();
            builder.RegisterType<TenantProvisioningService>();
            builder.RegisterType<AuthenticationService>();

            builder.RegisterType<AccessApplicationService>();
            builder.RegisterType<IdentityAccessEventProcessor>();
            builder.RegisterType<IdentityApplicationService>();
            builder.RegisterType<NotificationApplicationService>();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            IContainer container = builder.Build();

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            
            ServiceLocator.Resolver = new AutofacResolver(container);
        }
    }
}
