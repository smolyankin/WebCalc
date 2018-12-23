using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCalc.Service;
using WebCalc.ViewModel;

namespace WebCalc.Controllers
{
    /// <summary>
    /// основной контроллер
    /// </summary>
    public class HomeController : Controller
    {
        private CalcService service = new CalcService();

        /// <summary>
        /// главная
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = new UserCalcViewModel();
            model.IpAddress = Request.UserHostAddress;
            model = service.GetUserHistory(model).GetAwaiter().GetResult();
            return View(model);
        }

        /// <summary>
        /// расчет
        /// </summary>
        /// <param name="model"></param>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(UserCalcViewModel model, FormCollection formCollection)
        {
            try
            {
                if (formCollection.Count >= 2)
                {
                    model.Input = formCollection[1].ToString();
                    if (!string.IsNullOrEmpty(model.Input))
                    {
                        model.IpAddress = Request.UserHostAddress;
                        model = service.Caltulate(model).GetAwaiter().GetResult();
                        model.Input = string.Empty;
                    }
                }

                if (formCollection.Count == 1)
                {
                    var strId = formCollection.Keys[0].ToString();
                    var id = long.Parse(strId);
                    model.IpAddress = Request.UserHostAddress;
                    model.RepeatId = id;
                    model = service.GetCalcById(model).GetAwaiter().GetResult();
                }
                return View(model);
            }
            catch
            {
                model.Message = "Введенные данные не являются математической формулой";
                model.IpAddress = Request.UserHostAddress;
                service.GetUserHistory(model).GetAwaiter().GetResult();
                return View(model);
            }
        }

        /// <summary>
        /// очистка истории пользователя
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Clear()
        {
            service.Clear(Request.UserHostAddress).GetAwaiter().GetResult();

            return Redirect("Index");
        }
    }
}