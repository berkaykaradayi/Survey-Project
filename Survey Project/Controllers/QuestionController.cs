using Survey_Project.Models;
using System;
//Berkay Karadayı
//201511034

using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Survey_Project.Controllers
{
    public class QuestionController : Controller
    {
        SurveyEntities db = new SurveyEntities();
        public ActionResult Index()
        {
            var model = db.Questions.ToList();
            return View(model);
        }

        public ActionResult Create(Questions questions)
        {
            //User user1 = new User();
            //user1.

            if (questions.Question != null)
            {
                questions.CreatedDate = DateTime.Now;
                questions.CreatedBy = "System";

                db.Questions.Add(questions);
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
            var model = db.Questions.Find(Id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Questions questions)// User değil Question
        {
            db.Entry(questions).State = System.Data.Entity.EntityState.Modified;
            db.Entry(questions).Property(e => e.CreatedBy).IsModified = false;
            //CreatedBy değiştirmeyelim diye
            db.Entry(questions).Property(e => e.CreatedDate).IsModified = false;
            //CreateDate değiştirmeyelim diye
            questions.ModifyBy = "System Edit";
            questions.ModifyDate = DateTime.Now;
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
            var questions = db.Questions.Find(Id);
            db.Questions.Remove(questions);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}