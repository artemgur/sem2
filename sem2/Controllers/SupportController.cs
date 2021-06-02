using System.Threading.Tasks;
using Authentication.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using sem2_FSharp;
using sem2.Models;
using SupportChat;

namespace sem2.Controllers
{
    public class SupportController: Controller
    {
        private readonly IChatDatabase chatDatabase;
        
        public SupportController(IChatDatabase chatDatabase)
        {
            this.chatDatabase = chatDatabase;
        }
        
        public async Task<ViewResult> SupportChat(int? userId = null)
        {
            if (User.HasSupportClaim() && userId != null)
                return View(new SupportChatDTO{Messages = await chatDatabase.GetMessages(userId.Value), UserId = userId});
            return View(new SupportChatDTO{Messages = await chatDatabase.GetMessages(User.GetId()), UserId = null});
        }

        public async Task<ViewResult> SupportAdminSelect()
        {
            if (User.HasSupportClaim())
                return View();
            return View("SupportChat");
        }

        public async Task<ViewResult> SupportAdmin(int userId)
        {
            return View(userId);
        }
    }
}