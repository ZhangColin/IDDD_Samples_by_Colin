using System.Security.Cryptography.X509Certificates;

namespace SaasOvation.Common.Persistence {
    public interface ICleanableStore {
        void Clean();
    }
}