using GuestBook.Models;
using GuestBook.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GuestBook.Controllers
{
    public class HomeController : Controller
    {
      
        IRepository repo;

        public HomeController(IRepository r)
        {
            repo = r;
        }
        public async Task<ActionResult> Index()
        {
            var messages = await repo.IncludeMessage(); 

            return View(messages);
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }


    }
}
