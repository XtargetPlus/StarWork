using BL;
using BL.db;
using BL.Model;
using BLCrypto;
using BLCrypto.Records;
using BLRequests.SubFunctions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLRequests.AuthRequests
{
    public class LoginRequest : IDisposable
    {
        private readonly DbApplicationContext _db;

        public LoginRequest(DbApplicationContext db)
        {
            _db = db;
        }

        public async Task<AuthInfo?> GetUser(string login, string password)
        {
            EGD egd = new();

            var ecnryptAuthValues = await _db.AuthInfos.Where(ai => ai.Login == login).Select(ai => new
            {
                ai.Password,
                ai.Salt,
                ai.Key,
                ai.IV,
            }).FirstOrDefaultAsync();

            if (ecnryptAuthValues != null)
            {
                if (egd.DecryptData.CheckEquelHashPasswords(
                    ecnryptAuthValues.Password, 
                    (await egd.DecryptData.DecryptAes(
                        ecnryptAuthValues.Salt,
                        egd.CreateKeyIv.GetKeyByte(password.UTF8GetBytes()),
                        ecnryptAuthValues.IV)).ConvertToSplitByte(), 
                    password) == 0)
                {
                    AuthInfo auth = new()
                    {
                        Login = login,
                        Key = (await egd.DecryptData.DecryptAes(
                                    ecnryptAuthValues.Key,
                                    egd.CreateKeyIv.GetKeyByte(password.UTF8GetBytes()),
                                    ecnryptAuthValues.IV)).ConvertToSplitByte(),
                        IV = ecnryptAuthValues.IV,
                        User = await _db.AuthInfos
                        .Include(ai => ai.User)
                        .Where(ai => ai.Login == login)
                        .Select(ai => new User
                        {
                            Id = ai.User.Id,
                            NickName = ai.User.NickName,
                            Age = ai.User.Age,
                            Phone = ai.User.Phone,
                            Email = ai.User.Email,
                            Note = ai.User.Note
                        })
                        .FirstOrDefaultAsync()
                    };
                    if (auth.User != null)
                    {
                        auth.User.Phone = (await egd.DecryptData.DecryptAes(
                            auth.User.Phone,
                            auth.Key,
                            auth.IV)).UTF8GetBytes();
                        return auth;
                    }
                }
            }
            return null;
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
