using SaasOvation.Common.Domain.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;
using SaasOvation.IdentityAccess.Domain.Identity.Model.User;
using SaasOvation.IdentityAccess.Domain.Identity.Repository;

namespace SaasOvation.IdentityAccess.Domain.Identity.Service {
    public class AuthenticationService {
        private readonly ITenantRepository _tenantRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEncryptionService _encryptionService;

        public AuthenticationService(ITenantRepository tenantRepository, IUserRepository userRepository,
            IEncryptionService encryptionService) {
            this._tenantRepository = tenantRepository;
            this._userRepository = userRepository;
            this._encryptionService = encryptionService;
        }

        public UserDescriptor Authenticate(TenantId tenantId, string userName, string password) {
            AssertionConcern.NotNull(tenantId, "TenantId must not be null.");
            AssertionConcern.NotEmpty(userName, "User name must be provided.");
            AssertionConcern.NotEmpty(password, "Password must be provided.");

            UserDescriptor userDescriptor = UserDescriptor.NullDescriptorInstance();

            Tenant tenant = _tenantRepository.Get(tenantId);

            if(tenant!=null && tenant.Active) {
                string encryptedPassword = _encryptionService.EncryptedValue(password);
                User user = this._userRepository.UserFromAuthenticCredentials(tenantId, userName, encryptedPassword);

                if(user!=null && user.IsEnabled) {
                    userDescriptor = user.UserDescriptor;
                }
            }

            return userDescriptor;
        }
    }
}