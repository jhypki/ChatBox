using Models;
using Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;

        public ChatService(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task<Chat> CreateChat(Chat chat)
        {
            return await _chatRepository.CreateChat(chat);
        }

        public async Task<Chat> CreateGroupChat(Chat chat)
        {
            return await _chatRepository.CreateGroupChat(chat);
        }

        public async Task<Chat> GetChatById(string id)
        {
            return await _chatRepository.GetChatById(id);
        }

        public async Task<List<Chat>> GetChatsByUserId(string userId)
        {
            return await _chatRepository.GetChatsByUserId(userId);
        }

        public async Task<List<Chat>> GetChatsByUserIds(string[] userIds)
        {
            return await _chatRepository.GetChatsByUserIds(userIds);
        }

        public async Task<List<Chat>> GetChats()
        {
            return await _chatRepository.GetChats();
        }

        public async Task<Chat> UpdateChat(Chat chat)
        {
            return await _chatRepository.UpdateChat(chat);
        }

        public async Task<bool> DeleteChat(string id)
        {
            return await _chatRepository.DeleteChat(id);
        }

        public async Task<Message> SendMessage(string chatId, Message message)
        {
            return await _chatRepository.SendMessage(chatId, message);
        }

        public async Task<bool> DeleteMessage(string chatId, string messageId)
        {
            return await _chatRepository.DeleteMessage(chatId, messageId);
        }

        public async Task<List<Message>> GetMessages(string chatId)
        {
            return await _chatRepository.GetMessages(chatId);
        }

        public async Task<Message> GetMessage(string chatId, string messageId)
        {
            return await _chatRepository.GetMessage(chatId, messageId);
        }
    }
}