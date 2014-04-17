using System.Security.Cryptography;
using System.Text;
using SaasOvation.Common.Domain.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Service;

namespace SaasOvation.IdentityAccess.Infrastructure {
    public class MD5EncryptionService: IEncryptionService {
        public string EncryptedValue(string plainTextValue) {
            AssertionConcern.NotEmpty(plainTextValue, "Plain text value to encrypt must be provided.");

            StringBuilder encryptedValue = new StringBuilder();
            MD5 hasher = MD5.Create();

            byte[] data = hasher.ComputeHash(Encoding.Default.GetBytes(plainTextValue));

            foreach(byte d in data) {
                encryptedValue.Append(d.ToString("x2"));
            }

            return encryptedValue.ToString();
        }
    }
}