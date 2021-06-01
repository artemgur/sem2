using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authentication.Infrastructure;
using Microsoft.AspNetCore.SignalR;
using sem2_FSharp;

namespace SupportChat
{
    public class ChatHub: Hub
    {
        private readonly IChatDatabase database;

        private readonly Dictionary<int, string> clientGroup;

        private readonly Dictionary<string, string> supportConnectionGroup;
        
        public ChatHub(IChatDatabase database)
        {
            this.database = database;
            database.ClearNotAnsweredUsers();
            clientGroup = new Dictionary<int, string>();
            supportConnectionGroup = new Dictionary<string, string>();
        }

        public async Task ListenUser(int user)
        {
            if (Context.User.HasSupportClaim())
            {
                supportConnectionGroup[Context.ConnectionId] = clientGroup[user];
        
                await database.RemoveNotAnsweredUser(user);
                
            }
        }
        
        public async Task Send(string message)
        {
            if (!Context.User.HasSupportClaim())
            {
                await Clients.Group(clientGroup[GetUserId()]).SendAsync("Receive", message, Context.ConnectionId);
                await database.AddMessage(GetUserId(), message, true);
            }
            else
            {
                var userId = GetUserFromGroup(supportConnectionGroup[Context.ConnectionId]);
                await Clients.Group(supportConnectionGroup[Context.ConnectionId]).SendAsync("Receive", message, Context.ConnectionId);
                if (userId != null)
                    await database.AddMessage(userId.Value, message, false);
            }
            
            //database.AddMessage()
        }
        
        // public async Task SendAdmin(string message, int userId)
        // {
        //     await Clients.Caller.SendAsync("Receive", message, Context.ConnectionId);
        //     //database.AddMessage()
        // }
        
        public override async Task OnConnectedAsync()
        {
            if (GetUserId() == -1)
            {
                return;
            }
            if (!Context.User.HasSupportClaim())
            {
                clientGroup[GetUserId()] = Guid.NewGuid().ToString();
                await database.AddNotAnsweredUser(GetUserId());
            }
            await base.OnConnectedAsync();
        }
        
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (!Context.User.HasSupportClaim())
            {
                var group = clientGroup[GetUserId()];
                clientGroup.Remove(GetUserId());
                await Clients.Group(group).SendAsync("Notify", $"{Context.ConnectionId} покинул в чат");
                await database.RemoveNotAnsweredUser(GetUserId());
            }
            else
            {
                var group = supportConnectionGroup[Context.ConnectionId];
                supportConnectionGroup.Remove(Context.ConnectionId);
                var userId = GetUserFromGroup(group);
                if (userId != null)
                    await database.AddNotAnsweredUser(userId.Value);
            }

            await database.RemoveNotAnsweredUser(GetUserId());
            //await Clients.All.SendAsync("Notify", $"{Context.ConnectionId} покинул в чат");
            await base.OnDisconnectedAsync(exception);
        }

        private int GetUserId() => Context.User.GetId();

        private int? GetUserFromGroup(string group)
        {
            var userIdPair = clientGroup.SingleOrDefault(x => x.Value == group);
            if (!userIdPair.Equals(default(KeyValuePair<int, string>)))
                return userIdPair.Key;
            return null;
        }
    }
}