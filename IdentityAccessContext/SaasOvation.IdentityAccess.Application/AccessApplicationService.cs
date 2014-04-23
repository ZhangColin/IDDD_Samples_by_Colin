using SaasOvation.IdentityAccess.Application.Commands;
using SaasOvation.IdentityAccess.Domain.Access.Model;
using SaasOvation.IdentityAccess.Domain.Access.Repository;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;
using SaasOvation.IdentityAccess.Domain.Identity.Model.User;
using SaasOvation.IdentityAccess.Domain.Identity.Repository;
using SaasOvation.IdentityAccess.Domain.Identity.Service;

namespace SaasOvation.IdentityAccess.Application {
    public class AccessApplicationService {
        private readonly IGroupRepository _groupRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ITenantRepository _tenantRepository;
        private readonly IUserRepository _userRepository;

        public AccessApplicationService(ITenantRepository tenantRepository, IGroupRepository groupRepository,
            IUserRepository userRepository, IRoleRepository roleRepository) {
            this._groupRepository = groupRepository;
            this._roleRepository = roleRepository;
            this._tenantRepository = tenantRepository;
            this._userRepository = userRepository;
        }

        public void AssignUserToRole(AssignUserToRoleCommand command) {
            TenantId tenantId = new TenantId(command.TenantId);
            User user = this._userRepository.UserWithUserName(tenantId, command.UserName);
            if(user!=null) {
                Role role = _roleRepository.RoleNamed(tenantId, command.RoleName);
                if(role!=null) {
                    role.AssignUser(user);
                }
            }
        }

        public bool IsUserInRole(string tenantId, string userName, string roleName) {
            return this.UserInRole(tenantId, userName, roleName) != null;
        }

        public User UserInRole(string tenantId, string userName, string roleName) {
            TenantId id = new TenantId(tenantId);
            User user = _userRepository.UserWithUserName(id, userName);
            if(user!=null) {
                Role role = this._roleRepository.RoleNamed(id, roleName);
                if(role!=null) {
                    if(role.IsInRole(user, new GroupMemberService(this._userRepository, this._groupRepository))) {
                        return user;
                    }
                }
            }
            return null;
        }

        public void ProvisionRole(ProvisionRoleCommand command) {
            TenantId tenantId = new TenantId(command.TenantId);
            Tenant tenant = _tenantRepository.Get(tenantId);
            Role role = tenant.ProvisionRole(command.RoleName, command.Description, command.SupportsNesting);
            _roleRepository.Add(role);
        }
    }
}