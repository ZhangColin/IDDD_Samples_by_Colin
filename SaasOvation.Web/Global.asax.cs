using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using Autofac;
using Autofac.Integration.Mvc;
using SaasOvation.Common;
using SaasOvation.IdentityAccess.Application;
using SaasOvation.IdentityAccess.Domain.Access.Repository;
using SaasOvation.IdentityAccess.Domain.Access.Service;
using SaasOvation.IdentityAccess.Domain.Identity.Repository;
using SaasOvation.IdentityAccess.Domain.Identity.Service;
using SaasOvation.IdentityAccess.Infrastructure;
using SaasOvation.IdentityAccess.Infrastructure.Persistence;

namespace SaasOvation.Web
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // 在应用程序启动时运行的代码
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

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
            builder.RegisterType<AuthorizationService>();

            builder.RegisterType<AccessApplicationService>();
            builder.RegisterType<IdentityAccessEventProcessor>();
            builder.RegisterType<IdentityApplicationService>();
            builder.RegisterType<NotificationApplicationService>();

            builder.Register(c => new SessionProvider("server=.;uid=sa;pwd=truth;Trusted_Connection=no;database=IDDD"));

            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            IContainer container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            ServiceLocator.Resolver = new AutofacResolver(container);
        }
    }
}