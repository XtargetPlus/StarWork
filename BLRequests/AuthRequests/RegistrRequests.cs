using BL;
using BL.Model;
using BLCrypto;
using BLCrypto.Records;
using BLRequests.SubFunctions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BLRequests.AuthRequests
{
    public class RegistrRequests : IDisposable
    {
        private DbApplicationContext _db { get; } = null!;

        public RegistrRequests(DbApplicationContext db)
        {
            _db = db;
        }
        public async Task<int> AddUser(string fullName, int age, string mobileNumber, string email, string password, string login)
        {
            EGD egd = new();
            var keyIv = egd.CreateKeyIv.GetKeyIV();

            (byte[] hashPassword, byte[] salt) = egd.EncryptData.HashValue(password);

            Role userRole = await _db.Roles.Where(r => r.Title == "user").Select(r => r).FirstAsync();
            AuthInfo authInfo = new()
            {
                Login = login,
                Password = hashPassword,
                Salt = await egd.EncryptData.EncryptAes(
                    salt.JoinComma(),
                    egd.CreateKeyIv.GetKeyByte(password.UTF8GetBytes()),
                    keyIv.Values.First()),
                Key = await egd.EncryptData.EncryptAes(
                    keyIv.Keys.First().JoinComma(),
                    egd.CreateKeyIv.GetKeyByte(password.UTF8GetBytes()),
                    keyIv.Values.First()),
                IV = keyIv.Values.First(),
                RoleId = userRole.Id,
                Role = userRole,
                User = new()
                {
                    Age = age,
                    Email = email,
                    Phone = await egd.EncryptData.EncryptAes(mobileNumber, keyIv.Keys.First(), keyIv.Values.First()),
                    NickName = fullName,
                    LastEntrance = null
                },
                Created = DateTime.Now
            };

            await _db.AuthInfos.AddAsync(authInfo);
            await _db.SaveChangesAsync();

            return 1;
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
