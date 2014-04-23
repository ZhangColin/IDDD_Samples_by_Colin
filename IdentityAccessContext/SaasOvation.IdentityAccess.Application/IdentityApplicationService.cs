using System;
using SaasOvation.Common.Domain.Model;
using SaasOvation.IdentityAccess.Application.Commands;
using SaasOvation.IdentityAccess.Domain.Identity.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Group;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;
using SaasOvation.IdentityAccess.Domain.Identity.Model.User;
using SaasOvation.IdentityAccess.Domain.Identity.Repository;
using SaasOvation.IdentityAccess.Domain.Identity.Service;

namespace SaasOvation.IdentityAccess.Application {
    public class IdentityApplicationService {
        private readonly AuthenticationService _authenticationService;
        private readonly GroupMemberService _groupMemberService;
        private readonly IGroupRepository _groupRepository;
        private readonly TenantProvisioningService _tenantProvisioningService;
        private readonly ITenantRepository _tenantRepository;
        private readonly IUserRepository _userRepository;

        public IdentityApplicationService(AuthenticationService authenticationService,
            GroupMemberService groupMemberService, TenantProvisioningService tenantProvisioningService,
            ITenantRepository tenantRepository, IGroupRepository groupRepository, IUserRepository userRepository) {
            this._authenticationService = authenticationService;
            this._groupMemberService = groupMemberService;
            this._groupRepository = groupRepository;
            this._tenantProvisioningService = tenantProvisioningService;
            this._tenantRepository = tenantRepository;
            this._userRepository = userRepository;
        }

        public void ActivateTenant(ActivateTenantCommand command) {
            Tenant tenant = GetExistingTenant(command.TenantId);
            tenant.Activate();
        }

        public void DeactivateTenant(DeactivateTenantCommand command) {
            Tenant tenant = GetExistingTenant(command.TenantId);
            tenant.Deactivate();
        }

        public UserDescriptor AuthenticateUser(AuthenticateUserCommand command) {
            return this._authenticationService.Authenticate(new TenantId(command.TenantId), command.UserName,
                command.Password);
        }

        public void ChangeUserContactInformation(ChangeContactInfoCommand command) {
            User user = GetExistingUser(command.TenantId, command.UserName);
            user.ChangePersonalContactInformation(new ContactInformation(new EmailAddress(command.EmailAddress),
                new PostalAddress(command.AddressStreetAddress, command.AddressCity, command.AddressStateProvince,
                    command.AddressPostalCode, command.AddressCountryCode),
                new Telephone(command.PrimaryTelephone), new Telephone(command.SecondaryTelephone)));
        }

        public void ChangeUserEmailAddress(ChangeEmailAddressCommand command) {
            User user = GetExistingUser(command.TenantId, command.UserName);
            user.ChangePersonalContactInformation(user.Person.ContactInformation
                .ChangeEmailAddress(new EmailAddress(command.EmailAddress)));
        }

        public void ChangeUserPostalAddress(ChangePostalAddressCommand command) {
            User user = GetExistingUser(command.TenantId, command.UserName);
            user.ChangePersonalContactInformation(user.Person.ContactInformation.ChangePostalAddress(new PostalAddress(
                command.AddressStreetAddress, command.AddressCity, command.AddressStateProvince,
                command.AddressPostalCode, command.AddressCountryCode)));

        }

        public void ChangeUserPrimaryTelephone(ChangePrimaryTelephoneCommand command) {
            User user = GetExistingUser(command.TenantId, command.UserName);
            user.ChangePersonalContactInformation(
                user.Person.ContactInformation.ChangePrimaryTelephone(new Telephone(command.Telephone)));
        }
        
        public void ChangeUserSecondaryTelephone(ChangeSecondaryTelephoneCommand command) {
            User user = GetExistingUser(command.TenantId, command.UserName);
            user.ChangePersonalContactInformation( 
                user.Person.ContactInformation.ChangeSecondaryTelephone(new Telephone(command.Telephone)));
        }

        public void ChangeUserPassword(ChangeUserPasswordCommand command) {
            User user = GetExistingUser(command.TenantId, command.UserName);
            user.ChangePassword(command.CurrentPassword, command.ChangedPassword);
        }

        public void ChangeUserPersonalName(ChangeUserPersonalNameCommand command) {
            User user = GetExistingUser(command.TenantId, command.UserName);
            user.ChangePersonalName(new FullName(command.FirstName, command.LastName));
        }

        public void DefineUserEnablement(DefineUserEnablementCommand command) {
            User user = GetExistingUser(command.TenantId, command.UserName);
            user.DefineEnablement(new Enablement(command.Enabled, command.StartDate, command.EndDate));
        }

        public Group GetGroup(string tenantId, string groupName) {
            return this._groupRepository.GroupNamed(new TenantId(tenantId), groupName);
        }

        public bool IsGroupMember(string tenantId, string groupName, string userName) {
            Group group = GetExistingGroup(tenantId, groupName);
            User user = GetExistingUser(tenantId, userName);
            return group.IsMember(user, this._groupMemberService);
        }

        public Group ProvisionGroup(ProvisionGroupCommand command) {
            Tenant tenant = GetExistingTenant(command.TenantId);
            Group group = tenant.ProvisionGroup(command.GroupName, command.Description);
            this._groupRepository.Add(group);
            return group;
        }

        public Tenant ProvisionTenant(ProvisionTenantCommand command) {
            return this._tenantProvisioningService.ProvisionTenant(command.TenantName, command.TenantDescription,
                new FullName(command.AdministorFirstName, command.AdministorLastName),
                new EmailAddress(command.EmailAddress), new PostalAddress(command.AddressStreetAddress,
                    command.AddressCity, command.AddressStateProvince, command.AddressPostalCode,
                    command.AddressCountryCode), new Telephone(command.PrimaryTelephone),
                new Telephone(command.SecondaryTelephone));
        }

        public User RegisterUser(RegisterUserCommand command) {
            Tenant tenant = GetExistingTenant(command.TenantId);
            User user = tenant.RegisterUser(command.InvitationIdentifier, command.UserName, command.Password,
                new Enablement(command.Enabled, command.StartDate, command.EndDate),
                new Person(new TenantId(command.TenantId), new FullName(command.FirstName, command.LastName),
                    new ContactInformation(new EmailAddress(command.EmailAddress), new PostalAddress(
                        command.AddressStreetAddress, command.AddressCity, command.AddressStateProvince,
                        command.AddressPostalCode, command.AddressCountryCode),
                        new Telephone(command.PrimaryTelephone), new Telephone(command.SecondaryTelephone))));

            if(user == null) {
                throw new InvalidOperationException("User not registered.");
            }

            this._userRepository.Add(user);

            return user;
        }

        public void AddGroupToGroup(AddGroupToGroupCommand command) {
            Group parentGroup = GetExistingGroup(command.TenantId, command.ParentGroupName);
            Group childGroup = GetExistingGroup(command.TenantId, command.ChildGroupName);
            parentGroup.AddGroup(childGroup, this._groupMemberService);
        }

        public void RemoveGroupFromGroup(RemoveGroupFromGroupCommand command) {
            Group parentGroup = GetExistingGroup(command.TenantId, command.ParentGroupName);
            Group childGroup = GetExistingGroup(command.TenantId, command.ChildGroupName);
            parentGroup.RemoveGroup(childGroup);
        }

        public void AddUserToGroup(AddUserToGroupCommand command) {
            Group group = GetExistingGroup(command.TenantId, command.GroupName);
            User user = GetExistingUser(command.TenantId, command.UserName);
            group.AddUser(user);
        }

        public void RemoveUserFromGroup(RemoveUserFromGroupCommand command) {
            Group group = GetExistingGroup(command.TenantId, command.GroupName);
            User user = GetExistingUser(command.TenantId, command.UserName);
            group.RemoveUser(user);
        }

        public User GetUser(string tenantId, string userName) {
            return this._userRepository.UserWithUserName(new TenantId(tenantId), userName);
        }

        private User GetExistingUser(string tenantId, string userName) {
            User user = this.GetUser(tenantId, userName);
            if(user==null) {
                throw new ArgumentException(string.Format("User does not exist for {0} and {1}.", tenantId, userName));
            }
            return user;
        }

        private Group GetExistingGroup(string tenantId, string groupName) {
            Group group = this.GetGroup(tenantId, groupName);
            if(group==null) {
                throw new ArgumentException(string.Format("Group does not exist for {0} and {1}.", tenantId, groupName));
            }
            return group;
        }

        public Tenant GetTenant(string tenantId) {
            return this._tenantRepository.Get(new TenantId(tenantId));
        }

        private Tenant GetExistingTenant(string tenantId) {
            Tenant tenant = this.GetTenant(tenantId);
            if(tenant == null) {
                throw new ArgumentException(string.Format("Tenant does not exist for: {0}.", tenantId));
            }

            return tenant;
        }

        public UserDescriptor GetUserDescriptor(string tenantId, string userName) {
            User user = this.GetUser(tenantId, userName);
            if(user!=null) {
                return user.UserDescriptor;
            }
            return null;
        }
    }
}