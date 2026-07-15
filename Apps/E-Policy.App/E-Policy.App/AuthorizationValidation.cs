using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

using Newtonsoft.Json;

namespace E_Policy.App
{
    public class AuthorizationValidation : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                if (actionContext.Request.Headers.Authorization == null)
                {
                    throw new Exception("Access Denied !");
                }

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsMaintenance"]))
                {
                    throw new Exception("Sorry, We Are Under Maintenance !");
                }

                string token = actionContext.Request.Headers.Authorization.Parameter;
            }
            catch (Exception ex)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                if(ex.Message.Contains("Maintenance")) actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest);

                var message = new { IsSuccess = false, Message = ex.Message };
                actionContext.Response.Content = new StringContent(JsonConvert.SerializeObject(message), System.Text.Encoding.UTF8, "application/json");
            }
        }
    }
}