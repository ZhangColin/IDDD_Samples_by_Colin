using System;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.IdentityAccess.Domain.Identity.Service {
    public class PasswordService {
        private const string _digits = "0123456789";
        private const string _letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const int _strongThreshold = 20;
        private const string _symbols = "\"`!?$?%^&*()_-+={[}]:;@'~#|\\<,>.?/";
        private const int _veryStrongThreshold = 40;

        public string GenerateStrongPassword() {
            string generatedPassword = null;
            string password = "";
            Random random = new Random();
            bool isStrong = false;
            int index = 0;

            while(!isStrong) {
                int opt = random.Next(4);
                switch(opt) {
                    case 0:
                        index = random.Next(_letters.Length);
                        password += _letters.Substring(index, 1);
                        break;
                    case 1:
                        index = random.Next(_letters.Length);
                        password += _letters.Substring(index, 1).ToLower();
                        break;
                    case 2:
                        index = random.Next(_digits.Length);
                        password += _digits.Substring(index, 1);
                        break;
                    case 3:
                        index = random.Next(_symbols.Length);
                        password += _symbols.Substring(index, 1);
                        break;
                }

                generatedPassword = password.ToString();
                if(generatedPassword.Length>7) {
                    isStrong = this.IsStrong(generatedPassword);
                }
            }

            return generatedPassword;
        }

        public bool IsStrong(string plainTextPassword) {
            return this.CalculatePasswordstrength(plainTextPassword) >= _strongThreshold;
        }

        public bool IsVeryStrong(string plainTextPassword) {
            return this.CalculatePasswordstrength(plainTextPassword) >= _veryStrongThreshold;
        }

        public bool IsWeak(string plainTextPassword) {
            return this.CalculatePasswordstrength(plainTextPassword) < _strongThreshold;
        }

        private int CalculatePasswordstrength(string plainTextPassword) {
            AssertionConcern.NotNull(plainTextPassword, "Password strength cannot be tested on null.");

            int strength = 0;
            int length = plainTextPassword.Length;
            if(length>7) {
                strength += 10;
                strength += (length - 7);
            }

            int digitCount = 0;
            int letterCount = 0;
            int lowerCount = 0;
            int upperCount = 0;
            int symbolCount = 0;

            for(int i = 0; i < length; i++) {
                char ch = plainTextPassword[i];

                if(char.IsLetter(ch)) {
                    letterCount++;
                    if(char.IsUpper(ch)) {
                        upperCount++;
                    }
                    else {
                        lowerCount++;
                    }
                }
                else if(char.IsDigit(ch)) {
                    digitCount++;
                }
                else {
                    symbolCount++;
                }
            }

            strength += (upperCount + lowerCount + symbolCount);

            if(letterCount>=2&& digitCount>=2) {
                strength += letterCount + digitCount;
            }

            return strength;
        }
    }
}