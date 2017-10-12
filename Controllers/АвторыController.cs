using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class АвторыController : Controller
    {
        private libraryEntities2 db = new libraryEntities2();

        // GET: Авторы
        public ActionResult Index()
        {
            return View(db.Авторы.ToList());
        }

        // GET: Авторы/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Авторы авторы = db.Авторы.Find(id);
            if (авторы == null)
            {
                return HttpNotFound();
            }
            return View(авторы);
        }
        // POST: Авторы/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Сотрудник")]
        public ActionResult Create([Bind(Include = "Код,ФИО_автора,Краткая_биография")] Авторы авторы, int parameter = 0)
        {
            if (ModelState.IsValid)
            {
                if (авторы.Краткая_биография == null) { авторы.Краткая_биография = "Нету"; }
                db.Авторы.Add(авторы);
                db.SaveChanges();
                if(parameter != 0)
                {
                    return RedirectToAction("Edit", "Книги",new { id = parameter });
                }
                return RedirectToAction("Create","Книги");
            }

            return View(авторы);
        }

        // GET: Авторы/Edit/5
        [Authorize(Roles = "Сотрудник")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Авторы авторы = db.Авторы.Find(id);
            if (авторы == null)
            {
                return HttpNotFound();
            }
            return View(авторы);
        }

        // POST: Авторы/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Сотрудник")]
        public ActionResult Edit([Bind(Include = "Код,ФИО_автора,Краткая_биография")] Авторы авторы)
        {
            if (ModelState.IsValid)
            {
                db.Entry(авторы).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details",routeValues: new { id = авторы.Код });
            }
            return View(авторы);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
