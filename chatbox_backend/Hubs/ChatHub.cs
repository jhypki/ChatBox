using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Hubs
{
    public class ChatHub : Hub
    {
        // In-memory storage to map chat IDs to user connections
        private static readonly Dictionary<string, List<string>> ChatConnections = new();

        // Called when a user connects to a specific chat
        public async Task JoinChat(string chatId)
        {
            if (!ChatConnections.ContainsKey(chatId))
            {
                ChatConnections[chatId] = new List<string>();
            }

            ChatConnections[chatId].Add(Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
            await Clients.Group(chatId).SendAsync("UserJoined", Context.ConnectionId);
        }

        // Called when a user leaves a specific chat
        public async Task LeaveChat(string chatId)
        {
            if (ChatConnections.ContainsKey(chatId))
            {
                ChatConnections[chatId].Remove(Context.ConnectionId);
                if (!ChatConnections[chatId].Any())
                {
                    ChatConnections.Remove(chatId);
                }
            }

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
            await Clients.Group(chatId).SendAsync("UserLeft", Context.ConnectionId);
        }

        // Sending a message to a specific chat
        public async Task SendMessageToChat(string chatId, string user, string message)
        {
            await Clients.Group(chatId).SendAsync("ReceiveMessage", user, message);
        }

        public override async Task OnDisconnectedAsync(System.Exception exception)
        {
            foreach (var chatId in ChatConnections.Keys)
            {
                if (ChatConnections[chatId].Contains(Context.ConnectionId))
                {
                    await LeaveChat(chatId);
                }
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
