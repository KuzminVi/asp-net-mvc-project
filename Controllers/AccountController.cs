using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        static int tries;
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        // GET
        [AllowAnonymous]
        public ActionResult Login()
        {
            tries = 0;
            return View();
        }
        // POST
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel l)
        {
             libraryEntities2 db = new libraryEntities2();
            var a = db.Пользователи.ToList();
            if (a.Exists(o => o.Имя == l.Имя && o.Пароль == l.Пароль) && tries <= 3)
            {
                FormsAuthentication.SetAuthCookie(l.Имя, false);

                var x = a.Find(o => o.Имя == l.Имя);
                x.Дата_последнего_входа = DateTime.UtcNow.Date;
                db.Entry(x).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            
            tries = tries + 1;
            if (tries > 3)
            {
                ViewBag.Error = "Превышено количество попыток(3)";
            }
            else
            {
                ViewBag.Error = "Неверные входные данные";
            }
            return View();
        }
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Register(RegisterViewModel r)
        {
            libraryEntities2 db = new libraryEntities2();
            if (db.Пользователи.ToList().Exists(o => o.Имя == r.Имя))
            {
                ModelState.AddModelError("Имя", "Имя уже взято");
            }
            if (db.Пользователи.ToList().Exists(o => o.Почта == r.Почта))
            {
                ModelState.AddModelError("Почта", "Адрес почты уже используется");
            }
            if (ModelState.IsValid)
            {
                db.Пользователи.Add(new Пользователи { Имя = r.Имя, Пароль = r.Пароль, Почта = r.Почта, Роль = "Пользователь", Дата_последнего_входа = DateTime.UtcNow.Date });
                db.SaveChanges();
                FormsAuthentication.SetAuthCookie(r.Имя, false);      
                return RedirectToAction("Index",controllerName:"Home");
            }
            return View();
        }
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

    }
}