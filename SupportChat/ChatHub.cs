using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authentication.Infrastructure;
using Microsoft.AspNetCore.SignalR;

namespace SupportChat
{
    public partial class ChatHub: Hub
    {
        private readonly IChatDatabase database;

        private static readonly Dictionary<int, int?> UsersToSupport = new ();

        private static readonly Dictionary<int, DateTime> UserLastMessage = new ();

        private const int UserTimeoutSeconds = 20 * 60; //20 min
        
        public ChatHub(IChatDatabase database)
        {
            this.database = database;
            UserCleaner.StartIfNotStarted(database);
        }

        public IEnumerable<int> GetNonAnsweredUsers()
        {
            if (Context.User.HasSupportClaim())
            {
                return UsersToSupport.Where(x => x.Value == null).Select(x => x.Key);
            }
            return Enumerable.Empty<int>();
        }
        
        public async Task Send(string message, int targetUser)
        {
            if (targetUser != -1)
            {
                await SendSupport(message, targetUser);
                return;
            }
            
            var userId = GetUserId();
            if (GetUserId() == -1)
            {
                return;
            }
            if (!UsersToSupport.ContainsKey(userId))
                UsersToSupport[userId] = null;
            if (UsersToSupport.ContainsKey(userId) && UsersToSupport[userId] != null)
            {
                await Clients.Users(UsersToSupport[userId].ToString()).SendAsync("Receive", message);
            }
            await database.AddMessage(userId, message, true);
            UserLastMessage[userId] = DateTime.Now;
        }
        
        public async Task SendSupport(string message, int targetUser)
        {
            var userId = GetUserId();
            if (Context.User.HasSupportClaim() && UsersToSupport.ContainsKey(targetUser))
            {
                UsersToSupport[targetUser] = userId; //Change in case of allowing multiple support members on 1 user
                await Clients.Users(targetUser.ToString()).SendAsync("Receive", message);
            }
            await database.AddMessage(targetUser, message, false);
        }

        public void DisconnectSupport(int targetUser)
        {
            if (Context.User.HasSupportClaim() && UsersToSupport.ContainsKey(targetUser))
            {
                UsersToSupport[targetUser] = null;
            }
        }

        public void UserEnd()
        {
            DeleteUserData(GetUserId(), database);
        }

        public void Disconnect(int targetUser)
        {
            if (targetUser == -1)
                UserEnd();
            else
                DisconnectSupport(targetUser);
        }

        private int GetUserId() => Context.User.GetId();

        private static async void DeleteUserData(int userId, IChatDatabase database)
        {
            UserLastMessage.Remove(userId);
            UsersToSupport.Remove(userId);
            await database.RemoveAllUserMessages(userId);
        }
    }
}