using Survey_Project.Models;
using Survey_Project.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Survey_Project.Controllers
{
    public class AnswerController : BaseController
    {
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
            {
                var model = db.Answers.Where(m=>m.UserCode == UserCode).ToList();
                return View(model);
            }
            else
            {
                var model = db.Answers.ToList();
                return View(model);
            }

            //var model = db.Answers.Where(m => m.UserCode == UserCode).ToList();
            //return View(model);

        }
        public ActionResult Create(string Code)
           
        {
            if (Code == null)
            {
                List<SelectListItem> userLİst =
                (from user in db.User // Do for Question too
                 where user.Code != UserCode
                 select new SelectListItem
                 {
                     Text = user.NameSurname,
                     Value = user.Code.ToString()
                 }).ToList();

                ViewBag.User = new SelectList
                    (userLİst.OrderBy(m => m.Text), "Value", "Text");


                var questionModel = db.Questions.ToList();

                return View(questionModel);

             //it will let user fill the survey
            
            }
            else 
            {  //will post in else
                CalculateScore(Code);
                return RedirectToAction("Index");
            }
            
        }
        public void CalculateScore(string code)
        {
            double yes = 0, no = 0;
            double result = 0;
            int res = 0;
            var answer = db.Answers.FirstOrDefault
                (m => m.PersonCode == code && m.UserCode==UserCode ); //"Code-> UserCode" here, logged user

            var answerLine = db.AnswerLine.Where(m => m.AnswerId == answer.Id).ToList();

            foreach (var item in answerLine)
            {
                if (item.Answers == Constants.AnswerType.Yes)
                {
                    yes++;
                }
                else
                {
                    no++;
                }              
            }
            result = ((yes / (yes + no))*100);
            res= (int)result;
            if (res > 75)
            {
                answer.IsComplete = true;
                
                // db.Answers.Add(Score);
         

            }
            else
            {
                answer.IsComplete = false;
                //answer.Score = result.ToString();
                // db.Answers.Add(Score);

            }


            answer.Score = res.ToString();
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }
            }


        }

        public String SendData(AnswerModel answerModel)
        {
            int? month = DateTime.Now.Month;

            var model = db.Answers.FirstOrDefault(m=>m.PersonCode == answerModel.Code && m.UserCode == UserCode &&
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
                answer.UserCode = UserCode;
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
                //answerLine.Id = answerId;///////////
                db.AnswerLine.Add(answerLine);
                db.SaveChanges();
            }

             

        }
    
        public ActionResult Detail(int? Id)
        {
            var model = db.AnswerLine.Where(m => m.AnswerId == Id).ToList();
            return View(model);
        }

    }
}