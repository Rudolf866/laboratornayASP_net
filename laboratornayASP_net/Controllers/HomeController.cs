using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using laboratornayASP_net.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace laboratornayASP_net.Controllers
{
    public class HomeController : Controller
    {
        private SiteContext db;
        public HomeController(SiteContext context)
        {
            db = context;
        }

        [HttpGet]
        public ActionResult Comment()
        {
            var query = from b in db.Comments
                        orderby b.CommentDate descending
                        select b;
            ViewBag.Comments = query;
            foreach (var item in query)
                db.Entry(item).Reference(p => p.ClientModel).Load();


            return View();
        }

        [HttpPost]
        // **123*[ValidateAntiForgeryToken]
        public async Task<IActionResult> Comment(Comment comment)
        {
            var query = from b in db.Comments
                        orderby b.CommentDate descending
                        select b;
            ViewBag.Comments = query;
            foreach (var item in query)
                db.Entry(item).Reference(p => p.ClientModel).Load();
            if (ModelState.IsValid)
            {
                Comment user = db.Comments.FirstOrDefault(u => u.Id == comment.Id);
                if (user == null)
                {
                    // добавляем пользователя в бд
                    db.Comments.Add(new Comment { UserName = comment.UserName, CommentDate = comment.CommentDate, CommentText = comment.CommentText, Hidden = comment.Hidden });
                    await db.SaveChangesAsync();
                    // db.SaveChanges();
                    return RedirectToAction("Comment", "Home");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            db.Comments.Add(comment);
            db.SaveChanges();
            return View(comment);

        }
        //реализация скрытия комментария
        public ActionResult HideComment(int Id)
        {
            Comment comment = db.Comments.Find(Id);
            if (comment != null)
            {

                comment.Hidden = !comment.Hidden;
               
                db.SaveChanges();

            }
            return RedirectToAction("Comment", "Home");
        }
        //удаление комментария
        public ActionResult DeleteComment(int Id)
        {
            Comment comment = db.Comments.Find(Id);
            if (comment != null)
            {

                comment.Hidden = !comment.Hidden;
                db.Comments.Remove(comment);
                db.SaveChanges();

            }

            return RedirectToAction("Comment", "Home");
        }
        // для редактирования комментария
        public ActionResult EditComment(int id)
        {
            var query = from b in db.Comments
                        orderby b.CommentDate descending
                        select b;
            ViewBag.Comments = query;
            foreach (var item in query)
                db.Entry(item).Reference(p => p.ClientModel).Load();
            Comment comment = db.Comments.Find(id);
            ViewBag.EditId = id;
            return View("~/Views/Home/Comment.cshtml", comment);
        }
        // удаление пользователя
        /*
        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                ClientModel phone = await db.Usersabc.FirstOrDefaultAsync(p => p.Id == id);
                if (phone != null)
                    return View(phone);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                ClientModel phone = await db.Usersabc.FirstOrDefaultAsync(p => p.Id == id);
                if (phone != null)
                {
                    db.Usersabc.Remove(phone);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Client", "Home");
                }
            }
            return NotFound();
        }
        */


        //*********************************
        [HttpGet]
        public ActionResult DeleteClient()
        {
            return View();
        }
        public ActionResult DeleteClient(int Id)
        {
            ClientModel user = db.Usersabc.Find(Id);
            if (user != null)
            {

                db.Usersabc.Remove(user);
                db.SaveChanges();

            }

            return RedirectToAction("Client", "Home");
        }



        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Login(LoginModel Usersabc)
        {

            if (ModelState.IsValid)
            {
                ClientModel user = db.Usersabc.FirstOrDefault(u => u.Login == Usersabc.Login && u.Password == Usersabc.Password);
                if (user != null)
                {

                    await Authenticate(Usersabc.Login); // аутентификация
                    return RedirectToAction("Index", "Home");
                    //return Redirect("/Home/Index");



                }
                else { ModelState.AddModelError("", "Некорректные логин и(или)пароль"); }

            }

            return View(Usersabc);

        }

        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
        new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
        new Claim(ClaimsIdentity.DefaultRoleClaimType, userName =="admin" ? "admin": "user")
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie",
           ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных cookies
            // await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(id));
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));

        }



        [Authorize(Roles = "admin")]
        public IActionResult Client()
        {
            return View(db.Usersabc.OrderBy(b => b.FIO).ToList());

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Register(ClientModel Userabc)
        {
            if (ModelState.IsValid)
            {

                ClientModel user = db.Usersabc.FirstOrDefault(u => u.FIO == Userabc.FIO);
                if (user == null)
                {
                    // добавляем пользователя в бд
                    db.Usersabc.Add(new ClientModel { FIO = Userabc.FIO, Address = Userabc.Address, Phone = Userabc.Phone, Email = Userabc.Email, Login = Userabc.Login, Password = Userabc.Password });
                    await db.SaveChangesAsync();
                    // db.SaveChanges();
                    await Authenticate(Userabc.Login); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
        
        db.Usersabc.Add(Userabc);
            db.SaveChanges();
            return View(Userabc);
        }

        public async Task<IActionResult> Logout()
        {
            // await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult Register()
        {
            

            return View();
        }




        public IActionResult Links()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
