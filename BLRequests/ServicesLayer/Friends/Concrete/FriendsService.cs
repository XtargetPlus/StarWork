using BL.Model;
using BL;
using BLRequests.DataLayer.BaseGenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BLRequests.Dtos.Friend;
using BLRequests.DataLayer;

namespace BLRequests.ServicesLayer.Friends.Concrete
{
    public class FriendsService : IFriendsService
    {
        private readonly DbApplicationContext _context;
        private readonly GenericRepository<User> _usersRepository;
        private readonly GenericRepository<Friend> _friendsRepository;

        public FriendsService(DbApplicationContext context)
        {
            _context = context;
            _usersRepository = new(_context);
            _friendsRepository = new(_context);
        }

        public async Task<List<string>?> GetAllUsersAsync(int id)
        {
            return id != 0 ? await new FriendRequest(_context).GetAllUsersAsync(id) : null;
        }

        public async Task<List<string>?> GetAllFriendsAsync(int id)
        {
            return id != 0 ? await _friendsRepository.GetAllAsync(select: f => f.UserFriend.NickName,
                filter: f => f.IsSuccess && f.MainId == id,
                take: int.MaxValue) : null;
        }

        public async Task<List<string>?> GetAllApplicationsAsync(int id)
        {
            return id != 0 ? await _friendsRepository.GetAllAsync(select: f => f.Main.NickName,
                filter: f => !f.IsSuccess && f.UserFriendId == id,
                take: int.MaxValue) : null;
        }
        
        public async Task<int?> AddAsync(string nick, int id)
        {
            User? user = await _usersRepository.FindFirstAsync(u => u.NickName == nick);
            if (user == null)
                return null;
            Friend? result = await _friendsRepository.CreateAsync(new Friend
            {
                MainId = id,
                UserFriend = user,
                IsSuccess = false
            });
            if (result == null) 
                return null;
            return 1;
        }

        public async Task<int?> AcceptAsync(string nick, int id)
        {
            if (id == 0)
                return null;
            Friend? newFriend = await _friendsRepository.CreateAsync(new Friend
            {
                MainId = id,
                UserFriend = await _usersRepository.FindFirstAsync(u => u.NickName == nick) ?? throw new ArgumentException(),
                IsSuccess = true
            });
            if (newFriend == null)
                return null;
            return await _friendsRepository.UpdateAsync(f => f.UserFriendId == id && f.Main.NickName == nick, 
                s => s.SetProperty(f => f.IsSuccess, true));
        }

        public async Task<int?> CancelAsync(string nick, int id)
        {
            if (id == 0)
                return null;
            Friend? friend = await _friendsRepository.FindFirstAsync(f => f.MainId == id && f.UserFriend.NickName == nick);
            if (friend == null) 
                return null;
            int? result = await _friendsRepository.RemoveAsync(friend);
            if (result < 1)
                return null;
            return 1;
        }

        public async Task<int?> DeleteAsync(string nick, int id)
        {
            if (id == 0)
                return null;
            Friend? friendFirst = await _friendsRepository.FindFirstAsync(f => f.MainId == id && f.UserFriend.NickName == nick);
            Friend? friendSecond = await _friendsRepository.FindFirstAsync(f => f.UserFriendId == id && f.Main.NickName == nick);
            if (friendFirst == null || friendSecond == null)
                return null;
            int result = await _friendsRepository.RemoveAsync(friendFirst);
            if (result < 1)
                return null;
            result = await _friendsRepository.RemoveAsync(friendSecond);
            if (result < 1)
                return null;
            return 1;
        }

        public async Task<List<GetAllFriendsDto>?> GetAllFriendsWithIdAsync(int id)
        {
            return id != 0 ? await _friendsRepository.GetAllAsync(select: f =>  new GetAllFriendsDto()
                {
                    Id = f.UserFriendId,
                    NickName = f.UserFriend.NickName
                },
                filter: f => f.IsSuccess && f.MainId == id,
                take: int.MaxValue) : null;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
