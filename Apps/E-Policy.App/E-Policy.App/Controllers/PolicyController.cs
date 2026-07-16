using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;

using Newtonsoft.Json;
using E_Policy.App.Models;

namespace E_Policy.App.Controllers
{
    public class PolicyController : BaseController
    {
        PolicyModel policyModel;

        public PolicyController()
        {
            policyModel = new PolicyModel();
        }

        [HttpGet]
        public IHttpActionResult Index()
        {
            return Ok("Welcome to E-Policy API !!!");
        }

        [AuthorizationValidation]
        [HttpGet]
        public HttpResponseMessage Download(string url = "")
        {
            HttpResponseMessage response;

            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    throw new Exception("No Parameter !");
                }

                Dictionary<string, object> result = policyModel.DownloadFile(url);
                response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new ByteArrayContent((byte[])result["File"]);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/zip");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = result["FileName"].ToString()
                };

                // expose Content-Disposition ke browser
                response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
            }
            catch (Exception ex)
            {
                var data = new { IsSuccess = false, Message = ex.Message };
                response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                response.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            }

            return response;
        }
    }
}
