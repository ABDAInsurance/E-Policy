using System;
using System.Collections.Generic;
using System.Web.Http;

using E_Policy.Models;

namespace E_Policy.Controllers
{
    public class RequestController : BaseController
    {
        RequestModel requestModel;

        public RequestController()
        {
            requestModel = new RequestModel();
        }

        [AuthorizationValidation]
        [HttpPost]
        public IHttpActionResult GetRequest(Dictionary<string, object> request)
        {
            try
            {
                object data = requestModel.GetRequest(request);
                return SuccessResponse(string.Empty, data);
            }
            catch (Exception ex)
            {
                return ErrorResponse("Request | GetRequest", ex.Message);
            }
        }
    }
}
