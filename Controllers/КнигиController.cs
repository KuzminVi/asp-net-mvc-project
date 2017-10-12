using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using System.Collections.Generic;

namespace WebApplication1.Controllers
{
    public class КнигиController : Controller
    {
        private libraryEntities2 db = new libraryEntities2();

        // GET: Книги
        [AllowAnonymous]
        public ActionResult Index(int orderbyrating = 0)
        {
            if (orderbyrating == 1)
            {
                var книги = db.Книги.Include(к => к.Авторы).ToList().FindAll(o=> o.Средний_рейтинг != null);
                книги.Sort(delegate (Книги x, Книги y)
                {
                    return x.Средний_рейтинг.Value.CompareTo(y.Средний_рейтинг.Value);
                });
                книги.AddRange(db.Книги.Include(к => к.Авторы).ToList().FindAll(o => o.Средний_рейтинг == null));
                ViewBag.order = orderbyrating;
                return View(книги);
            }
            else if (orderbyrating == 2)
            {
                var книги = db.Книги.Include(к => к.Авторы).ToList().FindAll(o=> o.Средний_рейтинг != null);
                книги.Sort(delegate (Книги x, Книги y)
                {
                    return x.Средний_рейтинг.Value.CompareTo(y.Средний_рейтинг.Value) * (-1);
                });
                книги.AddRange(db.Книги.Include(к => к.Авторы).ToList().FindAll(o => o.Средний_рейтинг == null));
                ViewBag.order = orderbyrating;
                return View(книги);
            }
            else
            {
                var книги = db.Книги.Include(к => к.Авторы).ToList();
                ViewBag.order = orderbyrating;
                return View(книги);
            }
            
        }
        public ActionResult AdvancedSearch()
        {
            return View();
        }
        [HttpPost][AllowAnonymous]
        public ActionResult Search(string searchname = "",string authorname="", string genre="", string series="")
        {

                var книги = (db.Книги.Include(к => к.Авторы)).ToList();
                var найденные = книги.FindAll(o => o.Название.ToLower().Contains(searchname.ToLower())
                                                && o.Авторы.ФИО_автора.ToLower().Contains(authorname.ToLower())
                                                && o.Жанр.ToLower().Contains(genre.ToLower())
                                                && o.Серия.ToLower().Contains(series.ToLower()));
                int i = найденные.Count;
                return View(найденные.ToList());
        }
        [AllowAnonymous]
        public ActionResult DownloadBook(int id)
        {
            var file = db.Книги.ToList().Find(o => o.Код == id);
                // Путь к файлу
                string full_file_path = Server.MapPath("~/Files/" + file.Ссылка_на_скачивание);
                // Тип файла - content-type
                string file_type = "application/pdf";
                // Имя файла - необязательно
                string file_name = file.Название + ".pdf";
                if (System.IO.File.Exists(full_file_path))
                {
                    return File(full_file_path, file_type, file_name);
                }
                else
                {
                    file.Ссылка_на_скачивание = "Нету";
                    db.Entry(file).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
        }
        // GET: Книги/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Книги книги = db.Книги.Find(id);
            if (книги == null)
            {
                return HttpNotFound();
            }
            if (User.Identity.IsAuthenticated)
            {
                var a = db.Отзывы.ToList().Find(o => o.Создатель == User.Identity.Name
                                                   && o.Код_книги == id);
                if (a != null)
                {
                    ViewBag.ReviewWritten = 1;
                    ViewBag.Review = a.Содержание;
                }
                else
                {
                    ViewBag.ReviewWritten = 0;
                    ViewBag.Review = "";
                }
            }
            return View(книги);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult ManageReviews(Отзывы o,string btnAction)
        {
            if (btnAction == "Добавить отзыв")
            {
                db.Отзывы.Add(o);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = o.Код_книги });
            }
            if (btnAction == "Удалить")
            {
                db.Отзывы.Remove(db.Отзывы.ToList().Find(x=> x.Создатель == o.Создатель && x.Код_книги == o.Код_книги));
                db.SaveChanges();
                return RedirectToAction("Details", new { id = o.Код_книги });
            }
            if (btnAction == "Изменить")
            {
                db.Entry(o).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = o.Код_книги });
            }
            return RedirectToAction("Details", new { id = o.Код_книги });
        }
        [Authorize(Roles = "Сотрудник")]
        public ActionResult DeleteReview(int code, string creator)
        {
            var a = db.Отзывы.ToList().Find(o => o.Код_книги == code && o.Создатель == creator);
            db.Отзывы.Remove(a);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = code });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult EditReview(Отзывы o)
        {
            db.Entry(o).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details", new { id = o.Код_книги });
        }

        // GET: Книги/Create
        [Authorize(Roles = "Сотрудник")]
        public ActionResult Create()
        {
            List<Авторы> m = new List<Авторы>();
            m.AddRange(db.Авторы);
            m.RemoveAt(0);
            m.Sort(delegate (Авторы x, Авторы y)
            {
                return x.ФИО_автора.CompareTo(y.ФИО_автора);
            });
            m.Insert(0, db.Авторы.ToList().ElementAt(0));
            ViewBag.сущАвтор = new SelectList((m), "Код", "ФИО_автора");
            return View();
        }

        // POST: Книги/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Сотрудник")]
        public ActionResult Create(КнигиCreateViewModel книги)
        {
            if (ModelState.IsValid)
            {
                Книги новКнига = new Книги();
                новКнига.Название = книги.Название;
                if (книги.Серия == null) { новКнига.Серия = "Нет серии"; } else { новКнига.Серия = книги.Серия; }
                if (книги.Жанр == null) { новКнига.Жанр = "Неопределён"; } else { новКнига.Жанр = книги.Жанр; }
                if (книги.Краткое_описание == null) { новКнига.Краткое_описание = "Нету"; } else { новКнига.Краткое_описание = книги.Краткое_описание; }
                if (книги.Ссылка_на_скачивание == null) { новКнига.Ссылка_на_скачивание = "Нету"; } else { новКнига.Ссылка_на_скачивание = книги.Ссылка_на_скачивание; }
                новКнига.Год_выпуска = книги.Год_выпуска;
                новКнига.Название = книги.Название;
                if (книги.новАвтор != null)
                {
                    if (книги.новАвтор.Trim(' ') != "")
                    {
                        Авторы a = new Авторы();
                        a.ФИО_автора = книги.новАвтор;
                        a.Краткая_биография = "Нету";
                        db.Авторы.Add(a);
                        db.SaveChanges();
                        новКнига.Автор = db.Entry(a).Entity.Код;
                    }
                }
                else
                {
                    новКнига.Автор = книги.сущАвтор;
                }
                db.Книги.Add(новКнига);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<Авторы> m = new List<Авторы>();
            m.AddRange(db.Авторы);
            m.RemoveAt(0);
            m.Sort(delegate (Авторы x, Авторы y)
            {
                return x.ФИО_автора.CompareTo(y.ФИО_автора);
            });
            m.Insert(0, db.Авторы.ToList().ElementAt(0));
            ViewBag.сущАвтор = new SelectList((m), "Код", "ФИО_автора");
            return View(книги);
        }

        // GET: Книги/Edit/5
        [Authorize(Roles = "Сотрудник")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Книги книги = db.Книги.Find(id);
            if (книги == null)
            {
                return HttpNotFound();
            }
            var a = new КнигиCreateViewModel();
            a.Год_выпуска = книги.Год_выпуска;a.Жанр = книги.Жанр;a.Код = книги.Код;a.Краткое_описание = книги.Краткое_описание;a.Название = книги.Название;a.Серия = книги.Серия;a.Ссылка_на_скачивание = книги.Ссылка_на_скачивание;a.сущАвтор = книги.Автор;a.Средний_рейтинг = книги.Средний_рейтинг;
            List<Авторы> m = new List<Авторы>();
            m.AddRange(db.Авторы);
            m.RemoveAt(0);
            m.Sort(delegate (Авторы x, Авторы y)
            {
                return x.ФИО_автора.CompareTo(y.ФИО_автора);
            });
            m.Insert(0, db.Авторы.ToList().ElementAt(0));
            ViewBag.сущАвтор = new SelectList((m), "Код", "ФИО_автора");
            return View(a);
        }

        // POST: Книги/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Сотрудник")]
        public ActionResult Edit(КнигиCreateViewModel книги)
        {
            if (ModelState.IsValid)
            {
                Книги новКнига = new Книги();
                новКнига.Код = книги.Код;
                новКнига.Название = книги.Название;
                if (книги.Серия == null) { новКнига.Серия = "Нет серии"; } else { новКнига.Серия = книги.Серия; }
                if (книги.Жанр == null) { новКнига.Жанр = "Неопределён"; } else { новКнига.Жанр = книги.Жанр; }
                if (книги.Краткое_описание == null) { новКнига.Краткое_описание = "Нету"; } else { новКнига.Краткое_описание = книги.Краткое_описание; }
                if (книги.Ссылка_на_скачивание == null) { новКнига.Ссылка_на_скачивание = "Нету"; } else { новКнига.Ссылка_на_скачивание = книги.Ссылка_на_скачивание; }
                новКнига.Год_выпуска = книги.Год_выпуска;
                новКнига.Средний_рейтинг = книги.Средний_рейтинг;
                if (книги.новАвтор != null)
                {
                    if (книги.новАвтор.Trim(' ') != "")
                    {
                        Авторы a = new Авторы();
                        a.ФИО_автора = книги.новАвтор;
                        a.Краткая_биография = "Нету";
                        db.Авторы.Add(a);
                        db.SaveChanges();
                        новКнига.Автор = db.Entry(a).Entity.Код;
                    }
                }
                else
                {
                    новКнига.Автор = книги.сущАвтор;
                }

                db.Entry(новКнига).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<Авторы> m = new List<Авторы>();
            m.AddRange(db.Авторы);
            m.RemoveAt(0);
            m.Sort(delegate (Авторы x, Авторы y)
            {
                return x.ФИО_автора.CompareTo(y.ФИО_автора);
            });
            m.Insert(0, db.Авторы.ToList().ElementAt(0));
            ViewBag.сущАвтор = new SelectList((m), "Код", "ФИО_автора");
            return View(книги);
        }

        // GET: Книги/Delete/5
        [Authorize(Roles = "Сотрудник")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Книги книги = db.Книги.Find(id);
            if (книги == null)
            {
                return HttpNotFound();
            }
            return View(книги);
        }

        // POST: Книги/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Сотрудник")]
        public ActionResult DeleteConfirmed(int id)
        {
            Книги книги = db.Книги.Find(id);
            db.Книги.Remove(книги);
            db.SaveChanges();
            return RedirectToAction("Index");
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
