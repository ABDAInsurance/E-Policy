using System;
using System.Net;
using System.Web.Http;

using NLog;

namespace E_Policy.Controllers
{
    public class BaseController : ApiController
    {
        protected Logger logging;
        protected const string SUCCESS_MESSAGE = "Successfully";
        protected const string FAILED_MESSAGE = "System Failed, Please Try Again Later !";
        protected const string HIDDEN_MESSAGE = "[HIDDENMESSAGE]";
        protected const string NO_LOG_MESSAGE = "[NOLOGMESSAGE]";

        public BaseController()
        {
            logging = LogManager.GetCurrentClassLogger();
        }

        protected IHttpActionResult SuccessResponse(string message = null, object data = null)
        {
            object responseMessage = new object();
            if (string.IsNullOrEmpty(message)) message = SUCCESS_MESSAGE;

            if (data == null)
            {
                responseMessage = new { IsSuccess = true, Message = message };
            }
            else
            {
                responseMessage = new { IsSuccess = true, Message = message, Data = data };
            }

            return Content(HttpStatusCode.OK, responseMessage);
        }

        protected IHttpActionResult ErrorResponse(string functionName, string message = null)
        {
            object responseMessage = new object();
         
            if (string.IsNullOrEmpty(message))
            {
                message = FAILED_MESSAGE;
            }
            else
            {
                if (message.Contains(NO_LOG_MESSAGE))
                {
                    message = message.Replace(NO_LOG_MESSAGE, string.Empty);
                }
                else if (message.Contains(HIDDEN_MESSAGE))
                {
                    message = message.Replace(HIDDEN_MESSAGE, string.Empty);
                    logging.Error(functionName + " | " + message);
                    message = FAILED_MESSAGE;
                }
                else
                {
                    logging.Error(functionName + " | " + message);
                }
            }

            responseMessage = new { IsSuccess = false, Message = message };
            return Content(HttpStatusCode.BadRequest, responseMessage);
        }
    }
}
