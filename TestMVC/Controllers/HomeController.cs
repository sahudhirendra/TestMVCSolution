using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TestMVC.WebApi;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Net;

namespace TestMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult JQErroe()
        {
            return View();
        }
        [HttpPost]
        public JsonResult JQErroe1(FormCollection form)
        {
            //employee.Name = form["number"];
            string number = Request.Form["txtNumber1"].ToString();
            int no = Convert.ToInt32(number);
            return new JsonResult { Data = no, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult WebAPICall()
        {
            ApiCall obj = new ApiCall();

            string Token = obj.GetAuthTokenId("9008796152", "1411");

            ViewBag.Message = "Your contact page.";
            string LocalFileLocation = @"H:\LakshyaWorkSpace\LakshyaAdminPortal\Images\Dhiren.png";
            string SignzyImgURL = obj.SignzyImageUpload(LocalFileLocation);
            ////string SignzyImgURL1 = obj.SignzyImageUpload1(LocalFileLocation, "infinity");
            //await obj.UploadImage();
            return View();
        }
        public ActionResult Signzy()
        {
            ApiCall obj = new ApiCall();
            byte[] videoArray = System.IO.File.ReadAllBytes(@"H:\LakshyaWorkSpace\LakshyaAdminPortal\Images\SelfiVideo.mp4");
            string base64VideoRepresentation = Convert.ToBase64String(videoArray);
            var videoDirectUrl = obj.UploadMediaFile("Dhiren.mp4", base64VideoRepresentation, "video");

            //byte[] imageArray = System.IO.File.ReadAllBytes(@"H:\LakshyaWorkSpace\LakshyaAdminPortal\Images\Dhiren.png");
            //string base64ImageRepresentation = Convert.ToBase64String(imageArray);
            //var imageDirectUrl = obj.UploadMediaFile("Dhiren.png", base64ImageRepresentation, "image");
            return View();
        }
        public ActionResult ShortUrl()
        {
            ApiCall obj = new ApiCall();
            //string strUrl = "https://lakshyaadminportal.azurewebsites.net/";
            string strUrl = "http://52.172.11.232:88/lakshayuat/swagger/ui/index#/";
            string ShortUrl = obj.ShrinkURL(strUrl);
            ViewBag.URL = ShortUrl;
            return View();
        }
    }
}