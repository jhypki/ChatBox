using Models;
using MongoDB.Driver;
using System;
using Data;

namespace Repositories;

public class ChatRepository : IChatRepository
{
    private readonly IMongoCollection<Chat> _chats;

    public ChatRepository(MongoDbService mongoDbService)
    {
        _chats = mongoDbService.Database.GetCollection<Chat>("Chats");
    }

    public async Task<Chat> CreateChat(Chat chat)
    {
        await _chats.InsertOneAsync(chat);
        return chat;
    }

    public async Task<Chat> CreateGroupChat(Chat chat)
    {
        await _chats.InsertOneAsync(chat);
        return chat;
    }
    public async Task<List<Chat>> GetChats()
    {
        return await _chats.Find(chat => true).ToListAsync();
    }

    public async Task<Chat> GetChatById(string id)
    {
        return await _chats.Find(chat => chat.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<Chat>> GetChatsByUserIds(string[] userIds)
    {
        return await _chats.Find(chat => chat.Users.Any(user => userIds.Contains(user.Id))).ToListAsync();
    }

    public async Task<bool> DeleteChat(string id)
    {
        var result = await _chats.DeleteOneAsync(chat => chat.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<List<Chat>> GetChatsByUserId(string userId)
    {
        return await _chats.Find(chat => chat.Users.Any(user => user.Id == userId)).ToListAsync();
    }

    public async Task<Chat> UpdateChat(Chat chat)
    {
        await _chats.ReplaceOneAsync(c => c.Id == chat.Id, chat);
        return chat;
    }

    public async Task<Message> SendMessage(string chatId, Message message)
    {
        var chat = await GetChatById(chatId);
        if (chat == null)
        {
            throw new Exception("Chat not found.");
        }
        chat.Messages.Add(message);
        await UpdateChat(chat);
        return message;
    }

    public async Task<bool> DeleteMessage(string chatId, string messageId)
    {
        var chat = await GetChatById(chatId);
        if (chat == null)
        {
            throw new Exception("Chat not found.");
        }
        var message = chat.Messages.FirstOrDefault(m => m.Id == messageId);
        if (message == null)
        {
            throw new Exception("Message not found.");
        }
        chat.Messages.Remove(message);
        await UpdateChat(chat);
        return true;
    }

    public async Task<List<Message>> GetMessages(string chatId)
    {
        var chat = await GetChatById(chatId);
        if (chat == null)
        {
            throw new Exception("Chat not found.");
        }
        return chat.Messages;
    }

    public async Task<Message> GetMessage(string chatId, string messageId)
    {
        var chat = await GetChatById(chatId);
        if (chat == null)
        {
            throw new Exception("Chat not found.");
        }
        var message = chat.Messages.FirstOrDefault(m => m.Id == messageId);
        if (message == null)
        {
            throw new Exception("Message not found.");
        }
        return message;
    }




}