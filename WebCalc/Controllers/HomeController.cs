﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCalc.Service;
using WebCalc.ViewModel;

namespace WebCalc.Controllers
{
    public class HomeController : Controller
    {
        private CalcService service = new CalcService();

        public ActionResult Index()
        {
            var model = new UserCalcViewModel();
            /*string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = Request.ServerVariables["REMOTE_ADDR"];
            }*/
            model.IpAddress = Request.UserHostAddress;
            service.GetUserHistory(model).GetAwaiter().GetResult();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(UserCalcViewModel model, FormCollection formCollection)
        {
            model.Input = formCollection[1].ToString();
            if (!string.IsNullOrEmpty(model.Input))
            {
                model.IpAddress = Request.UserHostAddress;
                model = service.Caltulate(model).GetAwaiter().GetResult();
                model.Input = string.Empty;
            }
            
            return View(model);
        }

        [HttpPost]
        public ActionResult Clear()
        {
            service.Clear(Request.UserHostAddress).GetAwaiter().GetResult();
            //model.IpAddress = Request.UserHostAddress;
            //service.GetUserHistory(model).GetAwaiter().GetResult();

            return Redirect("Index");
        }
    }
}