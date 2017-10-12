using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.DataVisualization.Charting;
using System.Web.Mvc;
using System.Web.Security;
using System.IO;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        libraryEntities2 db = new libraryEntities2();
        public ActionResult Index()
        {
            var a = db.Пользователь_Книга.ToList().FindAll(o => o.Пользователь == User.Identity.Name);
            var b = new List<ManageViewModel>();
            foreach (var i in a)
            {
                string r;
                if (i.Рейтинг == null) { r = "-"; } else { r = i.Рейтинг.ToString(); }
                b.Add(new ManageViewModel { Код_книги = i.Код_книги, Статус = i.Статус, Рейтинг = r, Книги = i.Книги });
            }
            return View(b);
        }
        public ActionResult addMessage()
        {
            Сообщения s = new Сообщения();
            return View(s);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult addMessage(Сообщения m)
        {
            if(!db.Пользователи.ToList().Exists(o => o.Имя == m.Получатель))
            {
                ModelState.AddModelError("Получатель", "Такого пользователя не существует");
            }
            if (ModelState.IsValid)
            {
                db.Сообщения.Add(m);
                db.SaveChanges();
            }
            return RedirectToAction("Messages");
        }
        public ActionResult ClearReceived()
        {
            var a = db.Сообщения.ToList().Find(o => o.Получатель == User.Identity.Name);
            while (a != null)
            {
                db.Сообщения.Remove(a);
                db.SaveChanges();
                a = db.Сообщения.ToList().Find(o => o.Получатель == User.Identity.Name);
            }
            return RedirectToAction("Messages");
        }
        public ActionResult Messages(int parameter = 0, MessageViewModel curr = null)
        {
            var b = new List<MessageViewModel>();
            if (parameter == 0)
            {
                var a = db.Сообщения.ToList().FindAll(o => o.Получатель == User.Identity.Name);
                foreach (var i in a)
                {
                    b.Add(new MessageViewModel { Код = i.Код, Пользователь = i.Отправитель, Дата_отправки = i.Дата_отправки, Содержание = i.Содержание });
                }
                
            }
            else
            {
                var a = db.Сообщения.ToList().FindAll(o => o.Отправитель == User.Identity.Name);
                foreach (var i in a)
                {
                    b.Add(new MessageViewModel { Код = i.Код, Пользователь = i.Получатель, Дата_отправки = i.Дата_отправки, Содержание = i.Содержание });
                }
            }
            b.Sort(delegate (MessageViewModel x, MessageViewModel y)
            {
                return x.Дата_отправки.CompareTo(y.Дата_отправки);
            });
            ViewBag.parameter = parameter;
            if (curr != null)
            {
                ViewBag.curr = curr;
            }
            else { ViewBag.curr = null; }
            return View(b);
        }
        public ActionResult EditBook(int id)
        {
            var a = db.Пользователь_Книга.ToList().Find(o => o.Пользователь == User.Identity.Name && o.Код_книги == id);
            if (a == null)
           { return HttpNotFound(); }
            else {
                return View(new ManageViewModel { Код_книги = id, Рейтинг = a.Рейтинг.ToString(), Статус = a.Статус });
            }
        }
        [HttpPost]
        public ActionResult EditBook(ManageViewModel m)
        {
            var a = db.Пользователь_Книга.ToList().Find(o => o.Пользователь == User.Identity.Name && o.Код_книги == m.Код_книги);
            if (ModelState.IsValid)
            {
                if (m.Рейтинг=="-")
                {
                    a.Рейтинг = null;
                }
                else
                {
                    a.Рейтинг = short.Parse(m.Рейтинг);
                }
                
                a.Статус = m.Статус;
                db.Entry(a).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult AddBook(int id)
        {
            var a = db.Книги.ToList().Exists(o => o.Код == id);
            if (a)
            {
                var b = db.Пользователь_Книга.ToList().Exists(o => o.Пользователь == User.Identity.Name && o.Код_книги == id);
                if (b)
                {
                    return RedirectToAction("EditBook", new { id = id });
                }
                else
                {
                    Пользователь_Книга x = new Пользователь_Книга();
                    x.Код_книги = id;
                    x.Пользователь = User.Identity.Name;
                    x.Рейтинг = null;
                    x.Статус = "Заинтересовала";
                    db.Entry(x).State = EntityState.Added;
                    db.SaveChanges();
                    return RedirectToAction("EditBook", new { id = id });
                }
            }
            return HttpNotFound();
        }
        public ActionResult DeleteBook(int? id)
        {
            var a = db.Пользователь_Книга.ToList().Find(o => o.Пользователь == User.Identity.Name && o.Код_книги == id);
            if (a == null)
            { return HttpNotFound(); }
            else {
                db.Пользователь_Книга.Remove(a);
                db.SaveChanges();
                return RedirectToAction("Index");
           }
        }
        public ActionResult Export()
        {
            StringBuilder s = new StringBuilder();
            var list = db.Пользователь_Книга.ToList().FindAll(o => o.Пользователь == User.Identity.Name);
            foreach (var i in list)
            {
                s.Append(i.Книги.Название + "\t\t\t| " + i.Книги.Авторы.ФИО_автора + "\t\t\t| " + i.Книги.Жанр + "\r\n");
            }
            var x = System.IO.File.CreateText("C:\\Users\\hp\\Documents\\temp.txt");
            x.Write(s);
            x.Close();
            return File("C:\\Users\\hp\\Documents\\temp.txt", contentType: "txt",fileDownloadName:"экспорт.txt");
        }
        public ActionResult GraphPopular()
        {
            var x = new Chart();
            var s = new Series();
            var area = new ChartArea();
            area.AxisX.IsLabelAutoFit = true;
            area.AxisX.LabelAutoFitStyle = LabelAutoFitStyles.LabelsAngleStep30;
            area.AxisX.LabelStyle.Enabled = true;
            area.AxisX.Interval = 1;
            area.AxisY.Interval = 1;
            Dictionary<string, int> stats = new Dictionary<string, int>();
            foreach (var i in db.Книги.ToList())
            {
                int value = db.Пользователь_Книга.ToList().FindAll(o => o.Код_книги == i.Код).Count;
                s.Points.AddXY(i.Название, value);
            }
            x.ChartAreas.Add(area);
            x.Series.Add(s);
            x.Width = 5000;
            x.Height = 5000;
            var returnStream = new MemoryStream();
            x.ImageType = ChartImageType.Png;
            x.SaveImage(returnStream);
            returnStream.Position = 0;
            return new FileStreamResult(returnStream, "image/png");
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