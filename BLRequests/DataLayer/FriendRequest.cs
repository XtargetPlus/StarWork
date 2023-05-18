using BL.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BLRequests.DataLayer
{
    public class FriendRequest
    {
        private readonly DbContext _db;
        private readonly DbSet<Friend> _friends;
        private readonly DbSet<User> _user;

        public FriendRequest(DbContext db)
        {
            _db = db;
            _friends = _db.Set<Friend>();
            _user = _db.Set<User>();
        }

        public async Task<List<string>?> GetAllUsersAsync(int id)
        {
            try
            {
                return await _user.Where(u => u.Id != id && !_friends.Where(f => f.MainId == id).Select(f => f.UserFriendId).Contains(u.Id))
                    .Select(u => u.NickName).ToListAsync();
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Ничего не найдено: {ex.Message}");
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine($"Отмена операции: {ex.Message}");
            }
            return null;
        }
    }
}
