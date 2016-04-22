using Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http.Filters;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace TodoMvcApi.Filters
{
    public class GlobalExceptionAttribute :
        ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionContext)
        {
            var exceptionType = actionContext.Exception.GetType();

            if (exceptionType == typeof(ValidationException))
            {
                // get the exception
                var exception = (ValidationException)actionContext.Exception;
                // get the request
                var request = actionContext.Request;
                // get the HttpConfiguration used for this request
                var config = request.GetConfiguration();
                // get the content negotiator
                var negotiator = config.Services.GetService(typeof(IContentNegotiator)) as IContentNegotiator;
                // get the available media formatters
                var formatters = config.Formatters;

                // use the content negotiator to find the formatter and media type
                var matchResult = negotiator.Negotiate(
                    typeof(IEnumerable<ValidationResult>),
                    request,
                    formatters);
                var bestMatchResult = matchResult.Formatter;
                var mediaType = matchResult.MediaType;

                // create the response message as a 
                // bad request with the validation results
                actionContext.Response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new ObjectContent<IEnumerable<ValidationResult>>(
                        exception.Errors,
                        bestMatchResult,
                        mediaType.MediaType)
                };
            }
            else
            {
                base.OnException(actionContext);
            }
        }
    }
}