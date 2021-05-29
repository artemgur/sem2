using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace sem2
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
    }
}