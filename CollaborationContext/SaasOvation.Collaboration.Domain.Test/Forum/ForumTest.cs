using System;
using NUnit.Framework;
using SaasOvation.Collaboration.Domain.Collaborators;
using SaasOvation.Collaboration.Domain.Forums.Model.Discussions;
using SaasOvation.Collaboration.Domain.Forums.Model.Forums;
using SaasOvation.Collaboration.Domain.Tenants;

namespace SaasOvation.Collaboration.Domain.Test.Forum {
    [TestFixture]
    public class ForumTest {
        private Forums.Model.Forums.Forum CreateForum() {
            return new Forums.Model.Forums.Forum(new TenantId("Tenant01234567"), new ForumId("Forum01234567"),
                new Creator("colin", "Colin Zhang", "colin@saasovation.com"),
                new Moderator("colin", "Colin Zhang", "colin@saasovation.com"), "Colin Zhang does DDD",
                "A set of discussions about DDD for anonymous developers.", null);
        }

        [Test]
        public void CreateForumTest() {
            
        }
    }
}