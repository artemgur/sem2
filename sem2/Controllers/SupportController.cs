using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using sem2_FSharp;
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
        
        public async Task<ViewResult> SupportChat()
        {
            return View(await chatDatabase.GetMessages(User.GetId()));
        }

        public async Task<ViewResult> SupportAdminSelect()
        {
            if (User.HasClaim("support", ""))
                return View();
            return View("SupportChat");
        }

        public async Task<ViewResult> SupportAdmin(int userId)
        {
            return View(userId);
        }
    }
}