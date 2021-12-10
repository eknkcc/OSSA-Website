
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OSSA_Website.Models.NotificationModels;
using OSSA_Website.Models.SharedModels;
using OSSA_Website.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OSSA_Website.Controllers
{
    public class HomeController : Controller
    {
 
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Policy()         
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SubmitContactForm(string namesurname, string email, string message, string usercode)
        {
            try
            {
                //Check captcha
                if (!Utility.Captcha.ValidateCaptchaCode("securityCodeContact", usercode, HttpContext))
                {
                    return base.Json(new SimpleResponse { Success = false, Message = "Incorrect security code entry." });
                }

                //Create email model
                SendEmailModel model = new SendEmailModel();
                model.Subject = "Contact form submission from anonymous user";
                model.Content = "Name surname: " + namesurname + ", Email:" + email + ", Message:" + message;

                //Send email to system Admin
                string jsonResponse = Utility.Request.Post(Program._settings.ApiGateway_Service_URL + "/PublicActions/Notification/SendPublicContactEmail", Utility.Serializers.SerializeJson(model));

                //Parse response
                SimpleResponse res = Utility.Serializers.DeserializeJson<SimpleResponse>(jsonResponse);

                if (res.Success == false)
                {
                    res.Message = "Currently we are unable to send your message. Please try again later.";
                }

                return Json(res);

            }
            catch (Exception ex)
            {
               
                return base.Json(new SimpleResponse { Success = false, Message = "An error occurred while proccesing your request." });
            }
        }

        [Route("get-captcha-image")]
        public IActionResult GetCaptchaImage(string code)
        {
            int width = 100;
            int height = 36;
            var captchaCode = Captcha.GenerateCaptchaCode();
            var result = Captcha.GenerateCaptchaImage(width, height, captchaCode);
            HttpContext.Session.SetString(code, result.CaptchaCode);
            Stream s = new MemoryStream(result.CaptchaByteData);
            return new FileStreamResult(s, "image/png");
        }
    }
}
