using System;
using NUnit.Framework;
using SaasOvation.Common;
using SaasOvation.Common.Domain.Model;
using SaasOvation.IdentityAccess.Domain.Access.Repository;
using SaasOvation.IdentityAccess.Domain.Identity.Repository;
using SaasOvation.IdentityAccess.Domain.Identity.Service;

namespace SaasOvation.IdentityAccess.Domain.Test {
    public abstract class RepositoryTest: IdentityAccessTest {
//        private SessionProvider _sessionProvider;
//        private ITransaction _transaction;
        protected IUserRepository UserRepository;
        protected IGroupRepository GroupRepository;
        protected IRoleRepository RoleRepository;
        protected ITenantRepository TenantRepository;
        protected GroupMemberService GroupMemberService;
        [SetUp]
        protected void SetUp() {
            AutofacConfig.InitializeForNH();

            this.UserRepository = ServiceLocator.GetService<IUserRepository>();
            this.GroupRepository = ServiceLocator.GetService<IGroupRepository>();
            this.RoleRepository = ServiceLocator.GetService<IRoleRepository>();
            this.TenantRepository = ServiceLocator.GetService<ITenantRepository>();
            this.GroupMemberService = ServiceLocator.GetService<GroupMemberService>();

            //            _sessionProvider = new SessionProvider();
//            _transaction = Session.BeginTransaction();

            DomainEventPublisher.Instance.Reset();

        }


        [TearDown]
        protected void TearDown() {
//            _transaction.Rollback();
//            _transaction = null;
//            Session.Clear();

        }
        

//        protected ISession Session {
//            get { return _sessionProvider.GetSession(); }
//        }
    }
}