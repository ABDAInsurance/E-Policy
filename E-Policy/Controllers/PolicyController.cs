using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;

using Newtonsoft.Json;
using E_Policy.Models;

namespace E_Policy.Controllers
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
        [HttpPost]
        public IHttpActionResult GetPolicy(Dictionary<string, object> request)
        {
            try
            {
                if(request == null)
                {
                    MessageException.NoLogMessageException("No Parameter !");
                }
                
                if (!request.ContainsKey("PolicyNo"))
                {
                    MessageException.NoLogMessageException("Policy Number is Required !");
                }

                if (string.IsNullOrEmpty(request["PolicyNo"].ToString()))
                {
                    MessageException.NoLogMessageException("CI Number / Polis Number Can't Empty !");
                }

                if (request.ContainsKey("StartCertificateNo"))
                {
                    if (string.IsNullOrEmpty(request["StartCertificateNo"].ToString())) request.Remove("StartCertificateNo");
                }

                if (request.ContainsKey("EndCertificateNo"))
                {
                    if (string.IsNullOrEmpty(request["EndCertificateNo"].ToString())) request.Remove("EndCertificateNo");
                }
                
                object data = policyModel.GetPolicy(request);
                return SuccessResponse(string.Empty, data);
            }
            catch (Exception ex)
            {
                return ErrorResponse("Policy | GetPolicy", ex.Message);
            }
        }

        [AuthorizationValidation]
        [HttpPost]
        public IHttpActionResult GenerateDocument(List<Dictionary<string, object>> requests)
        {
            try
            {
                if (requests == null)
                {
                    MessageException.NoLogMessageException("No Parameter !");
                }

                object data = policyModel.GenerateDocument(requests);
                return SuccessResponse(string.Empty, data);
            }
            catch (Exception ex)
            {
                return ErrorResponse("Policy | GenerateDocument", ex.Message);
            }
        }

        [AuthorizationValidation]
        [HttpGet]
        public HttpResponseMessage Download(string source = "")
        {
            HttpResponseMessage response;

            try
            {
                if (string.IsNullOrEmpty(source))
                {
                    throw new Exception("No Parameter !");
                }

                Dictionary<string, object> result = policyModel.DownloadFile(source);
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
