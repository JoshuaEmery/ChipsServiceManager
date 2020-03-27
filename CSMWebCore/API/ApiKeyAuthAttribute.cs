using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.API
{
    //This class is a custom attribute like [Authorized] or [Required]
    //Whenver a controller with the [ApiKeyAuth] tag is accessed  the OnActionExecutionAsync 
    //Method will run

    //This specifies that this Attribute can only be used on methods and classes
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]

    //This inherits from Attribute and Implements IAsyncActionFilter which is where the
    //OnActionExecutionAsync came from
    public class ApiKeyAuthAttribute : Attribute, IAsyncActionFilter
    {
        //This is the name of the HTTP header where we are going to look for the key
        private const string ApiKeyHeaderName = "ApiKey";
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //Try to get a value from the http header "ApiKey" and store it in potentialApiKey
            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var potentialApiKey))
            {
                //If the try fails set the Result to Unauthorized
                context.Result = new UnauthorizedResult();
                return;
            }

            //This gets the configuration object from the IConfiguration that is created in Startup.CS
            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            //This pulls the value from the "ApiKey": in appsettings.json
            var apiKey = configuration.GetValue<string>("ApiKey");
            //compare the ApiKey from appsettings.json with the ApiKey given to the http header
            if (!apiKey.Equals(potentialApiKey))
            {
                //if they dont match set result to unauthorized
                context.Result = new UnauthorizedResult();
                return;
            }            
            await next();          
        }
    }
}
