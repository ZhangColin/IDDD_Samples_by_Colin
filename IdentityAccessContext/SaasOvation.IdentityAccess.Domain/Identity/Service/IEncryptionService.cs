namespace SaasOvation.IdentityAccess.Domain.Identity.Service {
    public interface IEncryptionService {
        string EncryptedValue(string plainTextPassword);
    }
}