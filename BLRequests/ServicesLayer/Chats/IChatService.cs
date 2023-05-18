using BLRequests.Dtos.Chat;
using BLRequests.Dtos.Message;
using BLRequests.ServicesLayer.Chats.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLRequests.ServicesLayer.Chats
{
    public interface IChatService : IDisposable
    {
        Task<int?> CreateAsync(AddChatInfoDto addChatInfo, int id, string password);
        Task<List<GetAllChatsDto>?> GetAllAsync(int id, string password);
        Task<List<GetAllMessagesDto>?> GetAllMessagesAsync(int chatId, int userId, string password);
        Task<GetChatInfoDto?> GetMoreInfoAsync(int chatId, int userId);
        Task<int?> ExpelUserAsync(ExpelUserDto userDto);
        Task<int?> UpdateInfoAsync(UpdateChatInfoDto infoDto);
        Task<MessageAfterCreated?> SendMessageAsync(SendMessageDto messageDto);
    }
}
