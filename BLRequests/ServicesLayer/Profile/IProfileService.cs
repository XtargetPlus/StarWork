using BLRequests.Dtos;
using BLRequests.Dtos.Profile;

namespace BLRequests.Repositories.Profile
{
    public interface IProfileService : IDisposable
    {
        Task<MyProfileInfoDto?> GetMyInfoAsync(int id);
        Task<ProfileInfoDto?> GetInfoAsync(string nick);
        Task<int?> DeleteUserAsync(int id);
        Task<int?> ChangePasswordAsync(int id, ChangingPasswordDto info);
        Task<int?> ChangeLoginAsync(int id, string login);
        Task<int?> UpdateInfoAsync(int id, MyProfileInfoDto profileInfo, EncryptDataDto encryptData = null!, string phone = null!);
    }
}
