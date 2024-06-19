using Models;

namespace Services;

public interface IChatService
{
    Task<Chat> CreateChat(Chat chat);
    Task<Chat> CreateGroupChat(Chat chat);
    Task<Chat> GetChatById(string id);
    Task<List<Chat>> GetChatsByUserId(string userId);
    Task<List<Chat>> GetChatsByUserIds(string[] userIds);
    Task<List<Chat>> GetChats();
    Task<Chat> UpdateChat(Chat chat);
    Task<bool> DeleteChat(string id);

    Task<Message> SendMessage(string chatId, Message message);
    Task<bool> DeleteMessage(string chatId, string messageId);
    Task<List<Message>> GetMessages(string chatId);
    Task<Message> GetMessage(string chatId, string messageId);

}