using SaasOvation.Common.Domain.Model;
using SaasOvation.IdentityAccess.Domain.Access.Model;
using SaasOvation.IdentityAccess.Domain.Access.Repository;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;
using SaasOvation.IdentityAccess.Domain.Identity.Model.User;
using SaasOvation.IdentityAccess.Domain.Identity.Repository;
using SaasOvation.IdentityAccess.Domain.Identity.Service;

namespace SaasOvation.IdentityAccess.Domain.Access.Service {
    public class AuthorizationService {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGroupRepository _groupRepository;

        public AuthorizationService(IUserRepository userRepository, IGroupRepository groupRepository,
            IRoleRepository roleRepository) {
            this._roleRepository = roleRepository;
            this._userRepository = userRepository;
            this._groupRepository = groupRepository;
        }


        public bool IsUserInRole(TenantId tenantId, string userName, string roleName) {
            AssertionConcern.NotNull(tenantId, "TenantId must not be null.");
            AssertionConcern.NotEmpty(userName, "User name must not be provided.");
            AssertionConcern.NotEmpty(roleName, "Role name must not be null.");

            User user = _userRepository.UserWithUserName(tenantId, userName);
            return user != null && this.IsUserInRole(user, roleName);
        }

        public bool IsUserInRole(User user, string roleName) {
            AssertionConcern.NotNull(user, "User must not be null.");
            AssertionConcern.NotEmpty(roleName, "Role name must not be null.");

            bool authorized = false;
            if (user.IsEnabled) {
                Role role = _roleRepository.RoleNamed(user.TenantId, roleName);
                if (role != null) {
                    authorized = role.IsInRole(user, new GroupMemberService(_userRepository, _groupRepository));
                }
            }

            return authorized;
        }
    }
}