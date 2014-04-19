using System;
using Autofac;
using NHibernate;
using NUnit.Framework;
using SaasOvation.Common;
using SaasOvation.Common.Domain.Model;
using SaasOvation.IdentityAccess.Domain.Access.Repository;
using SaasOvation.IdentityAccess.Domain.Identity.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;
using SaasOvation.IdentityAccess.Domain.Identity.Model.User;
using SaasOvation.IdentityAccess.Domain.Identity.Repository;
using SaasOvation.IdentityAccess.Domain.Identity.Service;
using SaasOvation.IdentityAccess.Infrastructure.Persistence;

namespace SaasOvation.IdentityAccess.Infrastructure.Test {
    public abstract class RepositoryTest {
        private SessionProvider _sessionProvider;
        private ITransaction _transaction;
        protected IUserRepository UserRepository;
        protected IGroupRepository GroupRepository;
        protected ITenantRepository TenantRepository;
        protected IRoleRepository RoleRepository;

        [SetUp]
        protected void SetUp() {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<MD5EncryptionService>().As<IEncryptionService>();
            builder.RegisterType<PasswordService>();
            IContainer container = builder.Build();
            ServiceLocator.Resolver = new AutofacResolver(container);


            _sessionProvider = new SessionProvider();
            this.UserRepository = new UserRepository(Session);
            this.GroupRepository = new GroupRepository(Session);
            this.TenantRepository = new TenantRepository(Session);
            this.RoleRepository = new RoleRepository(Session);
            _transaction = Session.BeginTransaction();

            DomainEventPublisher.Instance.Reset();

        }


        [TearDown]
        protected void TearDown() {
            _transaction.Rollback();
            _transaction = null;
            Session.Clear();

        }


        protected ISession Session {
            get { return _sessionProvider.GetSession(); }
        }


        protected Tenant CreateTenant() {
            TenantId tenantId = new TenantId(Guid.NewGuid().ToString());

            return new Tenant(tenantId, "Test Tenant", "This is a test tenant.", true);
        }

        protected User CreateUser(Tenant tenant) {
            DateTime now = DateTime.Now;
            RegistrationInvitation registrationInvitation =
                tenant.OfferRegistrationInvitation("Today-and-Tomorrow").WillStartOn(now).LastingUntil(now.AddDays(1));
            return tenant.RegisterUser(registrationInvitation.InvitationId,
                "colin", "secretPassword!", Enablement.IndefiniteEnablement(), PersonEntity(tenant));
        }

        protected static Person PersonEntity(Tenant tenant) {
            return new Person(tenant.TenantId, new FullName("Colin", "Zhang"),
                new ContactInformation(new EmailAddress("colin@saasovation.com"),
                    new PostalAddress("123 Pearl Street", "Boulder", "CO", "80301", "US"),
                    new Telephone("303-555-1210"), new Telephone("303-555-1212")));
        }
    }
}