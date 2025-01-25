using Microsoft.AspNetCore.DataProtection;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace MediaSoft.Services
{
    public class CryptoService
    {
        private readonly IDataProtector _protector;

        public CryptoService(IDataProtectionProvider provider, string target)
        {
            _protector = provider.CreateProtector(target);  
        }

        public string Encrypt(string plainText)
        {
            return _protector.Protect(plainText);
        }

        public string Decrypt(string cipherText)
        {
            return _protector.Unprotect(cipherText);
        }
    }
}
