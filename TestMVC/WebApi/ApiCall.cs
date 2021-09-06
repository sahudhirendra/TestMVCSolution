using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace TestMVC.WebApi
{
    public class ApiCall
    {
        public string GetAuthTokenId(string Phone, string pin)
        {
            string apiUrl = "https://lakshaywireless.com/LakshayApis/api/Authentication";
            var Login = new
            {
                PhoneNumber = Phone,
                PIN = pin,
            };
            string inputJson = (new JavaScriptSerializer()).Serialize(Login);
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = System.Text.Encoding.UTF8;
            string json = client.UploadString(apiUrl + "/Login", inputJson);
            JObject data = JObject.Parse(json);
            string TokenValue = (string)data["TokenResponse"]["Token"];
            client.Dispose();
            return TokenValue;
        }
        public string SignzyImageUpload(string FileName)
        {
            //ServicePointManager.Expect100Continue = true;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11;

            String uriString = "https://persist.signzy.tech/api/files/upload";
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "image/png";
            client.Headers["ttl"] = "infinity";
            byte[] responseArray = client.UploadFile(uriString, FileName);
            ASCIIEncoding ascii = new ASCIIEncoding();
            string decoded = ascii.GetString(responseArray);
            JObject data = JObject.Parse(decoded);
            string directURL = (string)data["file"]["directURL"];
            client.Dispose();
            return directURL;
        }
        public async Task<string> UploadImage()
        {
            var url = "https://persist.signzy.tech/api/files/upload";
            var file = @"H:\LakshyaWorkSpace\LakshyaAdminPortal\Images\Dhiren.png";
            string responsestr = string.Empty;
            try
            {


                //read file into upfilebytes array
                var upfilebytes = File.ReadAllBytes(file);

                //create new HttpClient and MultipartFormDataContent
                HttpClient client = new HttpClient();
                MultipartFormDataContent content = new MultipartFormDataContent();
                //byteArray Image
                ByteArrayContent baContent = new ByteArrayContent(upfilebytes);
                client.DefaultRequestHeaders.Add("ttl", "infinity");
                content.Add(baContent, "File", "Dhiren.png");

                //upload MultipartFormDataContent content async and store response in response var
                var response = await client.PostAsync(url, content);

                //read response result as a string async into json var
                responsestr = response.Content.ReadAsStringAsync().Result;

                //debug
                Debug.WriteLine(responsestr);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Caught: " + ex.ToString());
            }

            return responsestr;
        }
        public string UploadMediaFile(string fileName, string imageByte, string contentType)
        {
            FileUploadResponse fileUploadResponse = new FileUploadResponse();
            var request = new FileUploadRequest
            {
                file = fileName,
                fileContent = imageByte,
                ttl = "1 month"
            };
            if (contentType.Equals("image"))
                fileUploadResponse = UploadImage(request);
            else if (contentType.Equals("video"))
                fileUploadResponse = UploadVideo(request);

            return fileUploadResponse.file.directURL;
        }
        public FileUploadResponse UploadImage(FileUploadRequest fileUploadRequest)
        {
            var requestUri = new Uri("https://persist.signzy.tech/api/files/upload");
            var memoryStream = new MemoryStream(Convert.FromBase64String(fileUploadRequest.fileContent));
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.ExpectContinue = true;

                MultipartFormDataContent content = new MultipartFormDataContent();
                content.Add(CreateFileContent(new StreamContent(memoryStream), fileUploadRequest.file, "image/jpeg"));
                content.Add(new StringContent(fileUploadRequest.ttl, Encoding.UTF8, "application/json"), "ttl");

                HttpResponseMessage httpResponseMessage = client.PostAsync(requestUri, content).Result;
                FileUploadResponse channelLoginResponseDto = httpResponseMessage.Content.ReadAsAsync<FileUploadResponse>().Result;

                return channelLoginResponseDto;
            }
        }
        private StreamContent CreateFileContent(StreamContent fileContent, string fileName, string contentType)
        {
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "file",
                FileName = fileName
            };
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            return fileContent;
        }
        public FileUploadResponse UploadVideo(FileUploadRequest fileUploadRequest)
        {
            var requestUri = new Uri("https://persist.signzy.tech/api/files/upload");
            var memoryStream = new MemoryStream(Convert.FromBase64String(fileUploadRequest.fileContent));
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.ExpectContinue = true;

                MultipartFormDataContent content = new MultipartFormDataContent();
                content.Add(CreateFileContent(new StreamContent(memoryStream), fileUploadRequest.file, "video/mp4"));
                content.Add(new StringContent(fileUploadRequest.ttl, Encoding.UTF8, "application/json"), "ttl");

                HttpResponseMessage httpResponseMessage = client.PostAsync(requestUri, content).Result;
                FileUploadResponse channelLoginResponseDto = httpResponseMessage.Content.ReadAsAsync<FileUploadResponse>().Result;
                return channelLoginResponseDto;
            }
        }
        public string ShrinkURL(string longUrl)
        {
            string URL;
            URL = "http://tinyurl.com/api-create.php?url=" + longUrl.ToLower();

            System.Net.HttpWebRequest objWebRequest;
            System.Net.HttpWebResponse objWebResponse;
            System.IO.StreamReader srReader;

            string strHTML;
            objWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(URL);
            objWebRequest.Method = "GET";

            objWebResponse = (System.Net.HttpWebResponse)objWebRequest.GetResponse();
            srReader = new System.IO.StreamReader(objWebResponse.GetResponseStream());
            strHTML = srReader.ReadToEnd();
            srReader.Close();
            objWebResponse.Close();
            objWebRequest.Abort();

            return (strHTML);
        }
        public string ShrinkURL1(string longUrl)
        {
            WebRequest request = WebRequest.Create("https://www.googleapis.com/urlshortener/v1/url?key=YOUR_API_KEY_HERE");
            request.Method = "POST";
            request.ContentType = "application/json";
            string requestData = string.Format(@"{{""longUrl"": ""{0}""}}", longUrl);
            byte[] requestRawData = Encoding.ASCII.GetBytes(requestData);
            request.ContentLength = requestRawData.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(requestRawData, 0, requestRawData.Length);
            requestStream.Close();

            WebResponse response = request.GetResponse();
            StreamReader responseReader = new StreamReader(response.GetResponseStream());
            string responseData = responseReader.ReadToEnd();
            responseReader.Close();

            var deserializer = new JavaScriptSerializer();
            var results = deserializer.Deserialize<GoogleResponse>(responseData);
            string strHTML;
            strHTML = results.Id;
            return strHTML;
        }
    }
    public class FileUploadRequest
    {
        public string file { get; set; }
        public string ttl { get; set; } = "1 month";
        public string fileContent { get; set; }
    }
    public class ImageUploadResponse
    {
        public string id { get; set; }
        public string filetype { get; set; }
        public string size { get; set; }
        public string directURL { get; set; }
    }
    public class FileUploadResponse
    {
        public ImageUploadResponse file { get; set; }
    }
    public class GoogleResponse
    {
        public string Kind { get; set; }
        public string Id { get; set; }
        public string LongUrl { get; set; }
    }
}