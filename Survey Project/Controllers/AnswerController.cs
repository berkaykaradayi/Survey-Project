using Survey_Project.Models;
using Survey_Project.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Survey_Project.Controllers
{
    public class AnswerController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            List<SelectListItem> userLİst = (from user in db.User // Do for Question too
                                             where user.Code != Code
                                             select new SelectListItem
                                             {
                                                 Text = user.NameSurname,
                                                 Value = user.Code.ToString()
                                             }).ToList();

            ViewBag.User = new SelectList(userLİst.OrderBy(m => m.Text), "Value", "Text");
            
            
            var questionModel = db.Questions.ToList();
            
            return View(questionModel);
        }

        public String SendData(AnswerModel answerModel)
        {
            int? month = DateTime.Now.Month;

            var model = db.Answers.FirstOrDefault(m=>m.PersonCode == answerModel.Code && m.UserCode ==Code &&
                m.CreateDate.Value.Month== month);
            if (model != null)
            {
                SaveAnswerLine(answerModel.Question, answerModel.Answer, model.Id);

                // if no data in db
            }
            else
            {
                Answers answer = new Answers(); //same names with data base
                answer.PersonCode = answerModel.Code;
                answer.PersonName = answerModel.NameSurname;
                answer.UserCode = Code;
                answer.CreateDate = DateTime.Now;
                answer.CreatedBy = NameSurname; //Logged User

                db.Answers.Add(answer);
                db.SaveChanges();
                SaveAnswerLine(answerModel.Question, answerModel.Answer, answer.Id);

            }

   
            return "True";
        }

        public void SaveAnswerLine(string question, string answer, int answerId)
        {   
            var model = db.AnswerLine.FirstOrDefault(m=>m.AnswerId == answerId && m.Question == question);
            if(model != null)
            {
                model.Answers = answer;
                db.SaveChanges();
            }
            else
            {
                AnswerLine answerLine = new AnswerLine();
                answerLine.AnswerId = answerId;
                answerLine.Answers = answer;
                answerLine.Question = question;

                db.AnswerLine.Add(answerLine);
                db.SaveChanges();
            }

             

        }
    }
}