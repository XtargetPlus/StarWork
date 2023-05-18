using BL;
using BL.Model;
using BLCrypto;
using BLCrypto.Records;
using BLRequests.DataLayer.BaseGenericRepository;
using BLRequests.Dtos;
using BLRequests.Dtos.Profile;
using BLRequests.SubFunctions;
using System.Text;

namespace BLRequests.Repositories.Profile.Concrete
{
    public class ProfileService : IProfileService
    {
        private readonly DbApplicationContext _context;
        private readonly GenericRepository<User> _profileRepository;
        private readonly GenericRepository<AuthInfo> _authInfoRepository;

        public ProfileService(DbApplicationContext context)
        {
            _context = context;
            _profileRepository = new(_context);
            _authInfoRepository = new(_context);
        }

        public async Task<int?> ChangeLoginAsync(int id, string login)
        {
            AuthInfo? authInfo = await _authInfoRepository.FindFirstAsync(ai => ai.UserId == id);
            if (authInfo == null)
                return null;
            authInfo.Login = login;
            int? result = await _authInfoRepository.UpdateAsync(authInfo);
            if (result < 1)
                return null;
            return 1;
        }

        public async Task<int?> ChangePasswordAsync(int id, ChangingPasswordDto info)
        {
            AuthInfo? authInfo = await _authInfoRepository.FindFirstAsync(ai => ai.UserId == id);
            if (authInfo == null)
                return null;

            EGD egd = new();

            (byte[] hashPassword, byte[] salt) = egd.EncryptData.HashValue(info.NewPassword);

            authInfo.Salt = await egd.EncryptData.EncryptAes(
                salt.JoinComma(),
                egd.CreateKeyIv.GetKeyByte(info.NewPassword.UTF8GetBytes()),
                authInfo.IV);
            authInfo.Key = await egd.EncryptData.EncryptAes(
                await egd.DecryptData.DecryptAes(
                    authInfo.Key,
                    egd.CreateKeyIv.GetKeyByte(info.OldPassword.UTF8GetBytes()),
                    authInfo.IV),
                egd.CreateKeyIv.GetKeyByte(info.NewPassword.UTF8GetBytes()),
                authInfo.IV);
            authInfo.Password = hashPassword;

            int? result = await _authInfoRepository.UpdateAsync(authInfo);
            if (result < 1)
                return null;
            return 1;
        }

        public async Task<int?> DeleteUserAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<MyProfileInfoDto?> GetMyInfoAsync(int id)
        {
            return await _profileRepository.FindFirstAsync(u => u.Id == id, u => new MyProfileInfoDto { Note = u.Note ?? "" });
        }

        public async Task<ProfileInfoDto?> GetInfoAsync(string nick)
        {
            return await _profileRepository.FindFirstAsync(u => u.NickName == nick, 
                u => new MyProfileInfoDto { NickName = u.NickName, Age = u.Age, Email = u.Email, Note = u.Note ?? "" });
        }

        public async Task<int?> UpdateInfoAsync(int id, MyProfileInfoDto profileInfo, EncryptDataDto encryptData = null!, string phone = null!)
        {
            User? user = await _profileRepository.FindByIdAsync(id);
            if (user == null)
                return null;
            byte[] encryptMobileNumber = null!;
            if (encryptData != null)
            {
                var encryptInfo = await _authInfoRepository.FindFirstAsync(ai => ai.Login == encryptData.Login, ai => new
                {
                    ai.User!.Phone,
                    ai.Key,
                    ai.IV
                });
                if (encryptInfo == null)
                    return null;

                EGD egd = new();

                byte[] key = (await egd.DecryptData.DecryptAes(
                    encryptInfo.Key,
                    egd.CreateKeyIv.GetKeyByte(encryptData.Password.UTF8GetBytes()),
                    encryptInfo.IV)).ConvertToSplitByte();
                if (await egd.DecryptData.DecryptAes(encryptInfo.Phone, key, encryptInfo.IV) != phone)
                    return null;
                encryptMobileNumber = await egd.EncryptData.EncryptAes(profileInfo.Phone, 
                    key, 
                    encryptInfo.IV);

            }
            user.NickName = profileInfo.NickName;
            user.Age = profileInfo.Age;
            user.Email = profileInfo.Email;
            user.Note = profileInfo.Note;
            if (encryptMobileNumber != null)
                user.Phone = encryptMobileNumber;

            int? result = await _profileRepository.UpdateAsync(user);
            if (result is null || result <= 0)
                return null;
            return 1;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
