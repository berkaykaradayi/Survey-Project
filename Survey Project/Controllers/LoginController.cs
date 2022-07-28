//Berkay Karadayı
//201511034

using Survey_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Survey_Project.Controllers
{
    public class LoginController : Controller
    {
        SurveyEntities db = new SurveyEntities();
        public ActionResult SignIn(string Code, string Password)
        {
            if (Code == null)
            {
                return View();

            }
        else
            {var persons = db.User.FirstOrDefault
                (m => m.Code == Code && m.Password == Password);
             if (persons != null)
                {
                Session["Code"]= persons.Code;
                Session["NameSurname"]= persons.NameSurname;
                return RedirectToAction("Create", "Answer"); 
             }
             else
                {   
                ViewBag.Error ="The username or password is incorrect please try again";
                return View();
                 }   

             }
            
           
        }

        public ActionResult LogOut()
        {
            Session.Abandon(); //Ends Sessions
            return RedirectToAction("SignIn", "Login");
        }
    }
}