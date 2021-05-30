using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using sem2_FSharp;

namespace SupportChat
{
    public class ChatHub: Hub
    {
        private readonly IChatDatabase database;

        private readonly Dictionary<int, int> listeners;
        
        public ChatHub(IChatDatabase database)
        {
            this.database = database;
            database.ClearActiveUsers();
            listeners = new Dictionary<int, int>();
        }

        public async Task ListenUser(int user)
        {
            if (Context.User.HasClaim("support", ""))
            {
                await database.RemoveActiveUser(user);
                listeners.Add(user, Context.User.GetId());
            }
        }
        
        public async Task Send(string message)
        {
            await Clients.Caller.SendAsync("Receive", message, Context.ConnectionId);
            //database.AddMessage()
        }
        
        public override async Task OnConnectedAsync()
        {
            if (!Context.User.HasClaim("support", ""))
                await database.AddActiveUser(Context.User.GetId());
            //await Clients.All.SendAsync("Notify", $"{Context.ConnectionId} вошел в чат");
            await base.OnConnectedAsync();
        }
        
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            listeners.Remove(Context.User.GetId());
            foreach (var pair in listeners.Where(x => x.Value == Context.User.GetId()))
            {
                await database.AddActiveUser(pair.Key);
            }
            await database.RemoveActiveUser(Context.User.GetId());
            //await Clients.All.SendAsync("Notify", $"{Context.ConnectionId} покинул в чат");
            await base.OnDisconnectedAsync(exception);
        }
    }
}