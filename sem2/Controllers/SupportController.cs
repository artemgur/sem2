using Microsoft.AspNetCore.Mvc;

namespace sem2.Controllers
{
    public class SupportController: Controller
    {
        public IActionResult SupportChat()
        {
            return View();
        }
    }
}