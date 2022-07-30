//Berkay Karadayı
//201511034


using Sitecore.FakeDb;
using Survey_Project.Models;
using Survey_Project.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Survey_Project.Controllers
{
    public class PersonController : BaseController
    {
        // SurveyEntities db = new SurveyEntities();

        public ActionResult Index()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }
            else
            {
                var model = db.User.ToList();
                return View(model);
            }

        }

        public ActionResult Create(User user, string Answer1)
        {
            //User user1 = new User();
            //user1.

            if (user.NameSurname != null)
            {
                user.CreateDate = DateTime.Now;
                user.CreatedBy = NameSurname;
                if (Answer1 == Constants.AnswerType.Yes)
                {
                    user.IsAdmin = true; // Modele eklendi gözüküyor ama 
                }
                else
                {
                    user.IsAdmin = false;
                }
                db.User.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }

        }

        public ActionResult Edit(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return HttpNotFound();
            }
            var model = db.User.Find(Id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(User user, string Answer1)
        {
            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            db.Entry(user).Property(e => e.CreatedBy).IsModified = false;
            //CreatedBy değiştirmeyelim diye
            db.Entry(user).Property(e => e.CreateDate).IsModified = false;
            //CreateDate değiştirmeyelim diye
            user.ModifyBy = NameSurname;
            user.ModifyDate = DateTime.Now;
            if (Answer1 == Constants.AnswerType.Yes)
            {
                user.IsAdmin = true; // Modele eklendi gözüküyor ama 
            }
            else
            {
                user.IsAdmin = false;
            }
            db.SaveChanges();
            return RedirectToAction("Index");// Create'de nereye gönderiyorsa,
                                             // yine Index sayfasına göndersin 
        }


        public ActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return HttpNotFound();
            }
            var user
                = db.User.Find(Id);
            db.User.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }


}