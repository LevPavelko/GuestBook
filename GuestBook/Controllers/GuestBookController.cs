using GuestBook.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using GuestBook.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GuestBook.Controllers
{
    public class GuestBookController : Controller
    {
        IRepository repo;

        public GuestBookController(IRepository r)
        {
            repo = r;
        }
       

        public ActionResult Login()
        {
           
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                
                var users = await repo.Login(loginModel.Login);
                if (users == null)
                {
                    ModelState.AddModelError("", "Wrong login or password!");
                    return View(loginModel);
                }
               
                string? salt = users.Salt;

                
                byte[] password = Encoding.Unicode.GetBytes(salt + loginModel.Password);

              
                byte[] byteHash = SHA256.HashData(password);

                StringBuilder hash = new StringBuilder(byteHash.Length);
                for (int i = 0; i < byteHash.Length; i++)
                    hash.Append(string.Format("{0:X2}", byteHash[i]));

                if (users.Password != hash.ToString())
                {
                    ModelState.AddModelError("", "Wrong login or password!");
                    return View(loginModel);
                }
                HttpContext.Session.SetString("FirstName", users.FirstName);
                HttpContext.Session.SetString("LastName", users.LastName);
                return RedirectToAction("Index", "Home");
            }
            return View(loginModel);
        }
       

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterModel reg)
        {
            if (ModelState.IsValid)
            {
                User user = new User();
                user.FirstName = reg.FirstName;
                user.LastName = reg.LastName;
                user.Login = reg.Login;

                byte[] saltbuf = new byte[16];

                RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
                randomNumberGenerator.GetBytes(saltbuf);

                StringBuilder sb = new StringBuilder(16);
                for (int i = 0; i < 16; i++)
                    sb.Append(string.Format("{0:X2}", saltbuf[i]));
                string salt = sb.ToString();

                
                byte[] password = Encoding.Unicode.GetBytes(salt + reg.Password);

                 
                byte[] byteHash = SHA256.HashData(password);

                StringBuilder hash = new StringBuilder(byteHash.Length);
                for (int i = 0; i < byteHash.Length; i++)
                    hash.Append(string.Format("{0:X2}", byteHash[i]));

                user.Password = hash.ToString();
                user.Salt = salt;
                repo.CreateUser(user);
                repo.Save();
                return RedirectToAction("Login");
            }

            return View(reg);
        }

        public async Task<IActionResult> Create()
        {
            Messages messages = new Messages();
            if (HttpContext.Session.GetString("LastName") != null
                && HttpContext.Session.GetString("FirstName") != null)
            {
                
                return View(messages);
            }
            else
                return RedirectToAction("Login", "GuestBook");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task <IActionResult> Create( Messages mess)
        {
            //if (HttpContext.Session.GetString("LastName") != null && HttpContext.Session.GetString("FirstName") != null)
            //{
            if (!ModelState.IsValid)
            {
                string firstName = HttpContext.Session.GetString("FirstName");
                string lastName = HttpContext.Session.GetString("LastName");

                
                var user = await repo.InOrOut(firstName, lastName);

               
                if (user != null)
                {
                    var message = new Messages
                    {
                        Id_User = user.Id,
                        Message = mess.Message,
                        MessageDate = DateTime.Now
                    };

                await repo.Create(message);
                await repo.Save();

                return RedirectToAction("Index", "Home");
            }
            else
            {

                ModelState.AddModelError(string.Empty, "User not found.");
                return RedirectToAction("Index");
            }
        }
            return View(mess);
               

                



            //}
            //return RedirectToAction("Index");


        }
    }
}
