using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SupportChat
{
    public class ChatHub: Hub
    {
        private readonly IChatDatabase database;

        public ChatHub(IChatDatabase database)
        {
            this.database = database;
        }
        
        public async Task Send(string message)
        {
            await Clients.Caller.SendAsync("Receive", message, Context.ConnectionId);
            //database.AddMessage()
        }
        
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("Notify", $"{Context.ConnectionId} вошел в чат");
            await base.OnConnectedAsync();
        }
        
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.All.SendAsync("Notify", $"{Context.ConnectionId} покинул в чат");
            await base.OnDisconnectedAsync(exception);
        }
    }
}