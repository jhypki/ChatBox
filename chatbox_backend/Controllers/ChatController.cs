using Models;
using Services;
using Microsoft.AspNetCore.Mvc;
using System;


namespace Controllers;

[ApiController]
[Route("api/[controller]")]

public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;
    private readonly IAuthService _authService;
    public ChatController(IChatService chatService, IAuthService authService)
    {
        _chatService = chatService;
        _authService = authService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateChat(string userId1, string userId2)
    {
        try
        {
            var user1 = await _authService.GetUserById(userId1);
            var user2 = await _authService.GetUserById(userId2);
            if (user1 == null || user2 == null)
            {
                throw new Exception("User not found.");
            }
            var chat = new Chat
            {
                Users = new List<User> { user1, user2 }
            };
            var createdChat = await _chatService.CreateChat(chat);
            return Ok(createdChat);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("create-group")]
    public async Task<IActionResult> CreateChat(string[] userIds)
    {
        try
        {
            var Users = await _authService.GetUsersByIds(userIds);
            if (Users.Count != userIds.Length)
            {
                throw new Exception("User not found.");
            }
            var chat = new Chat
            {
                Users = Users
            };
            var createdChat = await _chatService.CreateChat(chat);
            return Ok(createdChat);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetChatById(string id)
    {
        try
        {
            var chat = await _chatService.GetChatById(id);
            return Ok(chat);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("get/user/{userId}")]
    public async Task<IActionResult> GetChatsByUserId(string userId)
    {
        try
        {
            var chats = await _chatService.GetChatsByUserId(userId);
            return Ok(chats);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("get/users")]
    public async Task<IActionResult> GetChatsByUserIds(string[] userIds)
    {
        try
        {
            var chats = await _chatService.GetChatsByUserIds(userIds);
            return Ok(chats);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetChats()
    {
        try
        {
            var chats = await _chatService.GetChats();
            return Ok(chats);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateChat(Chat chat)
    {
        try
        {
            var updatedChat = await _chatService.UpdateChat(chat);
            return Ok(updatedChat);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteChat(string id)
    {
        try
        {
            var result = await _chatService.DeleteChat(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{chatId}/send-message")]
    public async Task<IActionResult> SendMessage(string chatId, Message message)
    {
        try
        {
            var result = await _chatService.SendMessage(chatId, message);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{chatId}/delete-message/{messageId}")]
    public async Task<IActionResult> DeleteMessage(string chatId, string messageId)
    {
        try
        {
            var result = await _chatService.DeleteMessage(chatId, messageId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{chatId}/get-messages")]
    public async Task<IActionResult> GetMessages(string chatId)
    {
        try
        {
            var messages = await _chatService.GetMessages(chatId);
            return Ok(messages);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{chatId}/get-message/{messageId}")]
    public async Task<IActionResult> GetMessage(string chatId, string messageId)
    {
        try
        {
            var message = await _chatService.GetMessage(chatId, messageId);
            return Ok(message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


}