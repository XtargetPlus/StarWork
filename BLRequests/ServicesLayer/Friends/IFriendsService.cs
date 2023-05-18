using BLRequests.Dtos.Friend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLRequests.ServicesLayer.Friends
{
    public interface IFriendsService : IDisposable
    {
        Task<List<string>?> GetAllUsersAsync(int id);
        Task<List<string>?> GetAllFriendsAsync(int id);
        Task<List<GetAllFriendsDto>?> GetAllFriendsWithIdAsync(int id);
        Task<List<string>?> GetAllApplicationsAsync(int id);
        Task<int?> AddAsync(string nick, int id);
        Task<int?> AcceptAsync(string nick, int id);
        Task<int?> CancelAsync(string nick, int id);
        Task<int?> DeleteAsync(string nick, int id);
    }
}
