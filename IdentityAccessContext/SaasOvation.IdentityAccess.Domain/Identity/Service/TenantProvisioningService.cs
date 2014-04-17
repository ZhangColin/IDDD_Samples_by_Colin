using System;
using SaasOvation.Common.Domain.Model;
using SaasOvation.IdentityAccess.Domain.Access.Model;
using SaasOvation.IdentityAccess.Domain.Access.Repository;
using SaasOvation.IdentityAccess.Domain.Identity.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;
using SaasOvation.IdentityAccess.Domain.Identity.Model.User;
using SaasOvation.IdentityAccess.Domain.Identity.Repository;

namespace SaasOvation.IdentityAccess.Domain.Identity.Service {
    public class TenantProvisioningService {
        private readonly ITenantRepository _tenantRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public TenantProvisioningService(ITenantRepository tenantRepository, IUserRepository userRepository,
            IRoleRepository roleRepository) {
            this._tenantRepository = tenantRepository;
            this._userRepository = userRepository;
            this._roleRepository = roleRepository;
        }

        public Tenant ProvisionTenant(string tenantName, string tenantDescription, FullName administorName,
            EmailAddress emailAddress, PostalAddress postalAddress, Telephone primaryTelephone,
            Telephone secondaryTelephone) {
            try {
                Tenant tenant = new Tenant(_tenantRepository.GetNextIdentity(), tenantName, tenantDescription, true);
                _tenantRepository.Add(tenant);

                RegisterAdministratorFor(tenant, administorName, emailAddress, postalAddress, primaryTelephone,
                    secondaryTelephone);

                DomainEventPublisher.Instance.Publish(new TenantProvisioned(tenant.TenantId));

                return tenant;
            }
            catch(Exception e) {
                throw new InvalidOperationException("Cannot provision tenant because: " + e.Message);
            }
        }

        private void RegisterAdministratorFor(Tenant tenant, FullName administorName, EmailAddress emailAddress,
            PostalAddress postalAddress, Telephone primaryTelephone, Telephone secondaryTelephone) {
            RegistrationInvitation invitation = tenant.OfferRegistrationInvitation("init").OpenEnded();
            string strongPassword = new PasswordService().GenerateStrongPassword();

            User admin = tenant.RegisterUser(invitation.InvitationId, "admin", strongPassword,
                Enablement.IndefiniteEnablement(), new Person(tenant.TenantId, administorName,
                    new ContactInformation(emailAddress, postalAddress, primaryTelephone, secondaryTelephone)));

            tenant.WithdrawInvitation(invitation.InvitationId);
            _userRepository.Add(admin);

            Role adminRole = tenant.ProvisionRole("Administrator", "Default" + tenant.Name + " administrator.");
            adminRole.AssignUser(admin);
            _roleRepository.Add(adminRole);

            DomainEventPublisher.Instance.Publish(new TenantAdministratorRegistered(tenant.TenantId, tenant.Name,
                administorName, emailAddress, admin.UserName, strongPassword));
        }
    }
}