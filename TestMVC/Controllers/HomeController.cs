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
using Paytm;

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
        public ActionResult PayTM()
        {
            test();
            Dictionary<string, string> body = new Dictionary<string, string>();
            Dictionary<string, string> head = new Dictionary<string, string>();
            Dictionary<string, Dictionary<string, string>> requestBody = new Dictionary<string, Dictionary<string, string>>();

            body.Add("MID", "hynsrC52439598587821");
            //body.Add("MID", "BqlWpd94756018960681");
            body.Add("linkType", "GENERIC");
            body.Add("linkDescription", "Test  Payment");
            body.Add("linkName", "Test");

            head.Add("tokenType", "AES");

            //  Generate  CheckSum  here  from  Paytm  Library.
            string paytmChecksum = Checksum.generateSignature(JsonConvert.SerializeObject(body), "S3U0iJ97IxBFhN3w"); 

            head.Add("signature", paytmChecksum);

            requestBody.Add("body", body);
            requestBody.Add("head", head);

            string post_data = JsonConvert.SerializeObject(requestBody);

            //For  Staging  url
            string url = "https://securegw-stage.paytm.in/link/create";

            //For  Production  url
            //string  url  =  "https://securegw.paytm.in/link/create";

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);

            webRequest.Method = "POST";
            webRequest.ContentType = "application/json";
            webRequest.ContentLength = post_data.Length;

            using (StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream()))
            {
                requestWriter.Write(post_data);
            }

            string responseData = string.Empty;

            using (StreamReader responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
            {
                responseData = responseReader.ReadToEnd();
                Console.WriteLine(responseData);
            }
            return View();
        }
        public ActionResult PayTM1()
        {
            string orderid = "D" + DateTime.Now.Ticks.ToString();
            Dictionary<String, String> paytmParams = new Dictionary<String, String>();
            /* Find your MID in your Paytm Dashboard at https://dashboard.paytm.com/next/apikeys */
            paytmParams.Add("MID", "hynsrC52439598587821");
            /* Find your WEBSITE in your Paytm Dashboard at https://dashboard.paytm.com/next/apikeys */
            paytmParams.Add("WEBSITE", "WEBSTAGING");
            /* Find your INDUSTRY_TYPE_ID in your Paytm Dashboard at https://dashboard.paytm.com/next/apikeys */
            paytmParams.Add("INDUSTRY_TYPE_ID", "Retail");
            /* WEB for website and WAP for Mobile-websites or App */
            paytmParams.Add("CHANNEL_ID", "WEB");
            /* Enter your unique order id */
            paytmParams.Add("ORDER_ID", orderid);
            /* unique id that belongs to your customer */
            paytmParams.Add("CUST_ID", "455451");
            /* customer's mobile number */
            paytmParams.Add("MOBILE_NO", "9008796152");
            /* customer's email */
            paytmParams.Add("EMAIL", "sahu.dhirendrakumar@gmail.com");
            /**
            * Amount in INR that is payble by customer
            * this should be numeric with optionally having two decimal points
*/
            paytmParams.Add("TXN_AMOUNT", "10");
            /* on completion of transaction, we will send you the response on this URL */
            paytmParams.Add("CALLBACK_URL", "https://www.youtube.com/");
            /**
            * Generate checksum for parameters we have
            * You can get Checksum DLL from https://developer.paytm.com/docs/checksum/
            * Find your Merchant Key in your Paytm Dashboard at https://dashboard.paytm.com/next/apikeys 
*/
            //String checksum = paytm.CheckSum.generateCheckSum("S3U0iJ97IxBFhN3w", paytmParams);
            string checksum = Checksum.generateSignature(JsonConvert.SerializeObject(paytmParams), "S3U0iJ97IxBFhN3w");
            /* for Staging */
            string url = "https://securegw.paytm.in/order/process";
            /* for Production */
            // String url = "https://securegw.paytm.in/order/process";
            /* Prepare HTML Form and Submit to Paytm */
            String outputHtml = "";
            outputHtml += "<html>";
            outputHtml += "<head>";
            outputHtml += "<title>Merchant Checkout Page</title>";
            outputHtml += "</head>";
            outputHtml += "<body>";
            outputHtml += "<center><h1>Please do not refresh this page...</h1></center>";
            outputHtml += "<form method='post' action='" + url + "' name='paytm_form'>";
            foreach (string key in paytmParams.Keys)
            {
                outputHtml += "<input type='hidden' name='" + key + "' value='" + paytmParams[key] + "'>";
            }
            outputHtml += "<input type='hidden' name='CHECKSUMHASH' value='" + checksum + "'>";
            outputHtml += "</form>";
            outputHtml += "<script type='text/javascript'>";
            outputHtml += "document.paytm_form.submit();";
            outputHtml += "</script>";
            outputHtml += "</body>";
            outputHtml += "</html>";
            Response.Write(outputHtml.ToString());
            return View();
        }
        public void test()
        {
            //https://developer.paytm.com/docs/api/send-payment-request-api/
            Dictionary<string, Dictionary<string, string>> requestBody = new Dictionary<string, Dictionary<string, string>>();

            Dictionary<string, string> body = new Dictionary<string, string>();
            body.Add("mid", "hynsrC52439598587821");
            body.Add("merchantOrderId", "BEKJBJK123");
            body.Add("amount", "1303.00");
            body.Add("posId", "S12341235");
            body.Add("userPhoneNo", "9008796152");

            /*
            * Generate checksum by parameters we have in body
            * You can get Checksum JAR from https://developer.paytm.com/docs/checksum/
            * Find your Merchant Key in your Paytm Dashboard at https://dashboard.paytm.com/next/apikeys
            */

            string checksum = Checksum.generateSignature(JsonConvert.SerializeObject(body), "S3U0iJ97IxBFhN3w");

            Dictionary<string, string> head = new Dictionary<string, string>();
            head.Add("clientId", "C11");
            head.Add("version", "v1");
            head.Add("signature", checksum);

            requestBody.Add("body", body);
            requestBody.Add("head", head);

            String post_data = JsonConvert.SerializeObject(requestBody);
            /* for Staging */
            string url = "https://securegw-stage.paytm.in/order/sendpaymentrequest";

            /* for Production */
            // URL url = new URL("https://securegw.paytm.in/order/sendpaymentrequest");

            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = "POST";
                webRequest.ContentType = "application/json";
                webRequest.ContentLength = post_data.Length;

                using (StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream()))
                {
                    requestWriter.Write(post_data);
                }

                string responseData = string.Empty;
                using (StreamReader responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
                {
                    responseData = responseReader.ReadToEnd();
                    Console.WriteLine(responseData);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }


        }
    }
}