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
    public class RequestController : BaseController
    {
        PolicyModel policyModel;

        public RequestController()
        {
            policyModel = new PolicyModel();
        }

        [AuthorizationValidation]
        [HttpPost]
        public IHttpActionResult Index(Dictionary<string, object> request)
        {
            try
            {
                if (request == null)
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

                if (!request.ContainsKey("StartCertificateNo")) request.Add("StartCertificateNo", "");
                if (!request.ContainsKey("EndCertificateNo")) request.Add("EndCertificateNo", "");

                policyModel.GenerateRequest(request);
                return SuccessResponse(string.Empty);
            }
            catch (Exception ex)
            {
                return ErrorResponse("Policy | GetPolicy", ex.Message);
            }
        }

        [AuthorizationValidation]
        [HttpPost]
        public IHttpActionResult GetRequest(Dictionary<string, object> request)
        {
            try
            {
                if (request == null)
                {
                    MessageException.NoLogMessageException("No Parameter !");
                }

                object data = policyModel.GetRequest(request);
                return SuccessResponse(string.Empty, data);
            }
            catch (Exception ex)
            {
                return ErrorResponse("Policy | GetRequest", ex.Message);
            }
        }

    }
}
