using System;
using System.Net;
using Microsoft.AspNetCore.Mvc.Filters;
using pumpk1n_backend.Enumerations;
using pumpk1n_backend.Exceptions;
using pumpk1n_backend.Responders;

namespace pumpk1n_backend.Attributes
{
    public class HandleExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            Console.WriteLine($"Message : {exception.Message}\nStackTrace : {exception.StackTrace}");

            if (exception.GetType().IsSubclassOf(typeof(CustomException)))
            {
                var customException = (CustomException) exception;
                context.Result = customException.Code == ErrorCode.NotModifiedException
                    ? ApiResponder.RespondStatusCode(HttpStatusCode.NotModified)
                    : ApiResponder.RespondHandledError(exception.Message, ((CustomException) exception).Code);
            }
            else
                context.Result = ApiResponder.RespondUnhandledError(exception.Message, ErrorCode.UnknownException);
        }
    }
}
