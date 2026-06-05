using System;
using System.Security.Cryptography;
using System.Text;

namespace DatDichVuSuaChuaNhaCua.Services
{
    public class AccountService : IAccountService
    {
        public string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes).ToLower();
        }
    }
}


