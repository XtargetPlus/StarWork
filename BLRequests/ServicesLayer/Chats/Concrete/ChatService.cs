using BL.Model;
using BL;
using BLRequests.DataLayer.BaseGenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLRequests.Dtos.Chat;
using BLCrypto;
using BLRequests.Dtos.Message;
using System.Runtime.CompilerServices;
using System.Data.SqlTypes;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore;
using BLRequests.Dtos.Friend;
using BLRequests.ServicesLayer.Chats.Records;
using BLCrypto.Records;
using BLRequests.SubFunctions;

namespace BLRequests.ServicesLayer.Chats.Concrete
{
    public class ChatService : IChatService
    {
        private readonly DbApplicationContext _context;
        private readonly GenericRepository<Chat> _chatsRepository;
        private readonly GenericRepository<Message> _messagesRepository;
        private readonly GenericRepository<UsersChats> _usersChatsRepository;
        private readonly GenericRepository<Friend> _friendsRepository;
        private readonly GenericRepository<AuthInfo> _authInfoRepository;

        public ChatService(DbApplicationContext context)
        {
            _context = context;
            _chatsRepository = new(_context);
            _friendsRepository = new(_context);
            _authInfoRepository = new(_context);
            _usersChatsRepository = new(_context);
            _messagesRepository = new(_context);
        }

        public async Task<int?> CreateAsync(AddChatInfoDto addChatInfo, int id, string password)
        {
            bool isInFriends = (await _friendsRepository.GetAllAsync(filter: f => addChatInfo.UsersId.Contains(f.UserFriendId),
                select: f => f.UserFriendId) ?? new()).Count == addChatInfo.UsersId.Count;
            if (!isInFriends)
                return null;
            var encryptInfo = await _authInfoRepository.FindFirstAsync(ai => ai.UserId == id, select: ai => new
            {
                ai.Key,
                ai.IV,
                ai.Password,
                ai.Salt
            });
            if (encryptInfo == null)
                return null;

            EGD egd = new();

            if (egd.DecryptData.CheckEquelHashPasswords(
                encryptInfo.Password, 
                (await egd.DecryptData.DecryptAes(encryptInfo.Salt,
                    egd.CreateKeyIv.GetKeyByte(password.UTF8GetBytes()),
                    encryptInfo.IV)).ConvertToSplitByte(), 
                password) == 0)
            {
                var keyIv = egd.CreateKeyIv.GetKeyIV();
                (byte[] hashKey, byte[] salt) = egd.EncryptData.HashValue(keyIv.Keys.First().JoinComma());

                Chat? chat = new()
                {
                    Title = addChatInfo.Title,
                    Note = addChatInfo.Note,
                    Key = hashKey,
                    IV = keyIv.Values.First(),
                    Salt = await egd.EncryptData.EncryptAes(
                        salt.JoinComma(),
                        keyIv.Keys.First(),
                        keyIv.Values.First()),
                    Created = DateTime.Now.Date
                };
                byte[] encryptKey = await egd.EncryptData.EncryptAes(
                    keyIv.Keys.First().JoinComma(),
                    (await egd.DecryptData.DecryptAes(
                        encryptInfo.Key,
                        egd.CreateKeyIv.GetKeyByte(password.UTF8GetBytes()),
                        encryptInfo.IV)).ConvertToSplitByte(),
                    encryptInfo.IV);
                UsersChats usersChats = new()
                {
                    Chat = chat,
                    UserId = id,
                    RoleId = 2,
                    IsSuccess = true,
                    Key = encryptKey
                };
                chat.UsersChats.Add(usersChats);
                encryptKey = await egd.EncryptData.EncryptAes(
                    keyIv.Keys.First().JoinComma(),
                    egd.CreateKeyIv.GetKeyByte(encryptInfo.Password),
                    encryptInfo.IV);
                foreach (int userId in addChatInfo.UsersId)
                {
                    chat.UsersChats.Add(new()
                    {
                        Chat = chat,
                        UserId = userId,
                        RoleId = 1,
                        IsSuccess = false,
                        Key = encryptKey
                    });
                }
                chat = await _chatsRepository.CreateAsync(chat);
                if (chat == null)
                    return null;
                return chat.Id;
            }
            return null;
        }

        public async Task<List<GetAllChatsDto>?> GetAllAsync(int id, string password)
        {
            List<GetAllChatsDto>? chats = await _usersChatsRepository.GetAllAsync(filter: uc => uc.UserId == id,
                select: uc => new GetAllChatsDto()
                {
                    Id = uc.ChatId,
                    Title = uc.Chat.Title
                });
            if (chats == null) 
                return null;
            List<GetAllChatsDto>? resultChats = new();

            EGD egd = new();

            var encryptUserInfo = await _authInfoRepository.FindFirstAsync(ai => ai.UserId == id, select: ai => new
            {
                ai.Key,
                ai.IV
            });
            if (encryptUserInfo == null)
                return null;

            byte[] userKey = (await egd.DecryptData.DecryptAes(
                    encryptUserInfo.Key,
                    egd.CreateKeyIv.GetKeyByte(password.UTF8GetBytes()),
                    encryptUserInfo.IV)).ConvertToSplitByte();

            for (int i = 0; i < chats.Count; i++)
            {
                var encryptUserChatKey = await _usersChatsRepository.FindFirstAsync(uc => uc.UserId == id && uc.ChatId == chats[i].Id,
                    select: uc => uc);
                if (encryptUserChatKey == null)
                    return null;

                if (encryptUserChatKey.IsSuccess == false)
                {
                    var adminInfo = await _usersChatsRepository.FindFirstAsync(uc => uc.ChatId == chats[i].Id && uc.RoleId == 2,
                        select: uc => new { uc.User.AuthInfos.First().Password, uc.User.AuthInfos.First().IV });
                    if (adminInfo == null)
                        return null;

                    encryptUserChatKey.Key = await egd.EncryptData.EncryptAes(
                        await egd.DecryptData.DecryptAes(
                            encryptUserChatKey.Key,
                            egd.CreateKeyIv.GetKeyByte(adminInfo.Password),
                            adminInfo.IV),
                        userKey,
                        encryptUserInfo.IV);

                    encryptUserChatKey.IsSuccess = true;

                    int result = await _usersChatsRepository.UpdateAsync(encryptUserChatKey);
                    if (result < 1)
                        return null;
                }

                GetLastMessageToChatsListDto? encryptLastMessage = (await _messagesRepository
                    .GetAllAsync(filter: m => m.ChatId == chats[i].Id,
                        select: m => new GetLastMessageToChatsListDto()
                        {
                            EncryptMessage = m.Text,
                            DateTime = m.DateTime
                        },
                        orderBy: ob => ob.OrderByDescending(m => m.DateTime)) ?? new()).FirstOrDefault();
                if (encryptLastMessage == null)
                    continue;

                var encryptChatInfo = await _chatsRepository.FindFirstAsync(c => c.Id == chats[i].Id, select: c => new
                {
                    c.Key,
                    c.IV,
                    c.Salt
                });
                if (encryptChatInfo == null)
                    return null;

                byte[] chatKey = (await egd.DecryptData.DecryptAes(
                    encryptUserChatKey.Key,
                    userKey,
                    encryptUserInfo.IV)).ConvertToSplitByte();
                if (egd.DecryptData.CheckEquelHashPasswords(encryptChatInfo.Key,
                    (await egd.DecryptData.DecryptAes(
                        encryptChatInfo.Salt,
                        chatKey,
                        encryptChatInfo.IV)).ConvertToSplitByte(),
                    chatKey.JoinComma()) != 0)
                    return null;

                string message = await egd.DecryptData.DecryptAes(
                    encryptLastMessage.EncryptMessage,
                    chatKey,
                    encryptChatInfo.IV);

                chats[i].LastMessage = message;
                chats[i].PostedTime = encryptLastMessage.DateTime.ToString("HH:mm");
            }

            return chats;
        }

        public async Task<List<GetAllMessagesDto>?> GetAllMessagesAsync(int chatId, int userId, string password)
        {
            EGD egd = new();

            var encryptUserInfo = await _authInfoRepository.FindFirstAsync(ai => ai.UserId == userId, select: ai => new
            {
                ai.Key,
                ai.IV
            });
            if (encryptUserInfo == null)
                return null;
            var encryptChatInfo = await _chatsRepository.FindFirstAsync(c => c.Id == chatId, select: c => new
            {
                c.Key,
                c.IV,
                c.Salt
            });
            if (encryptChatInfo == null)
                return null;
            var encryptUserChatKey = await _usersChatsRepository.FindFirstAsync(uc => uc.UserId == userId && uc.ChatId == chatId,
                select: uc => uc);
            if (encryptUserChatKey == null)
                return null;

            List<GetAllMessagesDto> messages = await _messagesRepository.GetAllAsync(filter: m => m.ChatId == chatId,
                select: m => new GetAllMessagesDto()
                {
                    Id = m.Id,
                    Message = m.Text.JoinComma(),
                    NickName = m.User!.NickName,
                    PostingTime = m.DateTime.ToString("HH:mm"),
                    IsHost = m.UserId == userId
                },
                orderBy: ob => ob.OrderBy(m => m.DateTime),
                take: int.MaxValue) ?? new();
            if (messages.Count == 0)
                return messages;

            byte[] chatKey = (await egd.DecryptData.DecryptAes(
                    encryptUserChatKey.Key,
                    (await egd.DecryptData.DecryptAes(
                        encryptUserInfo.Key,
                        egd.CreateKeyIv.GetKeyByte(password.UTF8GetBytes()),
                        encryptUserInfo.IV)).ConvertToSplitByte(),
                    encryptUserInfo.IV)).ConvertToSplitByte();

            if (egd.DecryptData.CheckEquelHashPasswords(encryptChatInfo.Key,
                (await egd.DecryptData.DecryptAes(
                    encryptChatInfo.Salt,
                    chatKey,
                    encryptChatInfo.IV)).ConvertToSplitByte(),
                chatKey.JoinComma()) != 0)
                return null;

            for (int i = 0; i < messages.Count; i++)
            {
                messages[i].Message = await egd.DecryptData.DecryptAes(
                    messages[i].Message.ConvertToSplitByte(),
                    chatKey,
                    encryptChatInfo.IV);
            }

            return messages;
        }

        public async Task<GetChatInfoDto?> GetMoreInfoAsync(int chatId, int userId)
        {
            return await _chatsRepository.FindFirstAsync(c => c.Id == chatId,
                select: c => new GetChatInfoDto()
                {
                    ChatId = c.Id,
                    UserId = userId,
                    ChatName = c.Title,
                    Note = c.Note,
                    UsersInChat = c.UsersChats.Select(uc => new GetChatUsers()
                    {
                        Id = uc.UserId,
                        NickName = uc.User.NickName,
                        Role = uc.RoleId
                    }).ToList()
                });
        }

        public async Task<int?> ExpelUserAsync(ExpelUserDto userDto)
        {
            int result = await _usersChatsRepository.RemoveAsync(await _usersChatsRepository
                                                .FindFirstAsync(uc => uc.UserId == userDto.UserId && uc.ChatId == userDto.ChatId) ?? new());
            if (result < 1)
                return null;
            return 1;
        }

        public async Task<int?> UpdateInfoAsync(UpdateChatInfoDto infoDto)
        {
            Chat? chat = await _chatsRepository.FindByIdAsync(infoDto.ChatId);
            if (chat == null)
                return null;

            (chat.Title, chat.Note) = (infoDto.ChatName, infoDto.Note);

            int result = await _chatsRepository.UpdateAsync(chat);
            if (result < 1)
                return null;
            return 1;
        }

        public async Task<MessageAfterCreated?> SendMessageAsync(SendMessageDto messageDto)
        {
            if (messageDto.Password == null || messageDto.UserId == null)
                return null;

            EGD egd = new();

            var encryptUserInfo = await _authInfoRepository.FindFirstAsync(ai => ai.UserId == messageDto.UserId, select: ai => new
            {
                ai.Key,
                ai.IV,
                ai.Password,
                ai.Salt
            });
            if (encryptUserInfo == null)
                return null;
            var encryptChatInfo = await _chatsRepository.FindFirstAsync(c => c.Id == messageDto.ChatId, select: c => new
            {
                c.Key,
                c.IV,
                c.Salt
            });
            if (encryptChatInfo == null)
                return null;
            var encryptUserChatKey = await _usersChatsRepository.FindFirstAsync(uc => uc.UserId == messageDto.UserId && uc.ChatId == messageDto.ChatId,
                select: uc => uc);
            if (encryptUserChatKey == null)
                return null;

            if (egd.DecryptData.CheckEquelHashPasswords(
                encryptUserInfo.Password,
                (await egd.DecryptData.DecryptAes(
                    encryptUserInfo.Salt,
                    egd.CreateKeyIv.GetKeyByte(messageDto.Password.UTF8GetBytes()),
                    encryptUserInfo.IV)).ConvertToSplitByte(), 
                messageDto.Password) != 0)
                return null;

            byte[] chatKey = (await egd.DecryptData
                    .DecryptAes(encryptUserChatKey.Key,
                    (await egd.DecryptData.DecryptAes(
                        encryptUserInfo.Key,
                        egd.CreateKeyIv.GetKeyByte(messageDto.Password.UTF8GetBytes()),
                        encryptUserInfo.IV)).ConvertToSplitByte(),
                    encryptUserInfo.IV)).ConvertToSplitByte();
            if (egd.DecryptData.CheckEquelHashPasswords(
                encryptChatInfo.Key,
                (await egd.DecryptData.DecryptAes(encryptChatInfo.Salt, chatKey, encryptChatInfo.IV)).ConvertToSplitByte(),
                chatKey.JoinComma()) != 0)
                return null;

            Message? message = new()
            {
                Text = await egd.EncryptData.EncryptAes(messageDto.Message, chatKey, encryptChatInfo.IV),
                IsRead = false,
                DateTime = DateTime.Now,
                UserId = (int)messageDto.UserId,
                ChatId = messageDto.ChatId
            };

            message = await _messagesRepository.CreateAsync(message);
            if (message == null)
                return null;

            return new() { MessageId = message.Id, DateTime = message.DateTime.ToString("HH:mm"), Text = messageDto.Message };
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
