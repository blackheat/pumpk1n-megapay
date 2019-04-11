using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using pumpk1n_backend.Enumerations;
using pumpk1n_backend.Models.ReturnModels;

namespace pumpk1n_backend.Responders
{
    public class JsonWithHttpCodeResult : JsonResult
    {
        private readonly HttpStatusCode _httpStatusCode;
        private readonly String _eTag;

        public JsonWithHttpCodeResult(object value) : base(value)
        {
            _httpStatusCode = HttpStatusCode.OK;
        }

        public JsonWithHttpCodeResult(object value, JsonSerializerSettings serializerSettings) : base(value, serializerSettings)
        {
            _httpStatusCode = HttpStatusCode.OK;
        }

        public JsonWithHttpCodeResult(object value, HttpStatusCode httpStatusCode) : base(value)
        {
            _httpStatusCode = httpStatusCode;
        }

        public JsonWithHttpCodeResult(object value, JsonSerializerSettings serializerSettings, HttpStatusCode httpStatusCode) : base(value, serializerSettings)
        {
            _httpStatusCode = httpStatusCode;
        }

        public JsonWithHttpCodeResult(object value, String eTag, HttpStatusCode httpStatusCode) : base(value)
        {
            _httpStatusCode = httpStatusCode;
            _eTag = eTag;
        }

        public JsonWithHttpCodeResult(object value, String eTag, JsonSerializerSettings serializerSettings, HttpStatusCode httpStatusCode) : base(value, serializerSettings)
        {
            _httpStatusCode = httpStatusCode;
            _eTag = eTag;
        }

        public override void ExecuteResult(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = (Int32) _httpStatusCode;

            if (!String.IsNullOrEmpty(_eTag))
                context.HttpContext.Response.Headers.Add("ETag", _eTag);

            base.ExecuteResult(context);
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = (Int32)_httpStatusCode;

            if (!String.IsNullOrEmpty(_eTag))
                context.HttpContext.Response.Headers.Add("ETag", _eTag);

            return base.ExecuteResultAsync(context);
        }
    }

    public class ResponseObject
    {
        public PaginationReturnModel PaginationReturnData { get; set; }
        public String ResponseType { get; set; }
        public Object Data { get; set; }
    }

    public class ErrorResponseObject : ResponseObject
    {
        public ErrorCode ErrorCode { get; set; }
    }

    public static class ApiResponder
    {
        public static IActionResult RespondStatusCode(HttpStatusCode statusCode)
        {
            return new StatusCodeResult((Int32) statusCode);
        }

        public static JsonResult RespondSuccess(Object data, String eTag = null, PaginationReturnModel paginationReturnModel = null)
        {
            var response = new ResponseObject
            {
                ResponseType = "success",
                Data = data,
                PaginationReturnData = paginationReturnModel
            };

            return new JsonWithHttpCodeResult(response, eTag, HttpStatusCode.OK);
        }

        public static JsonResult RespondHandledError(Object data, ErrorCode errorCode)
        {
            var response = new ErrorResponseObject
            {
                ResponseType = "error",
                Data = data,
                ErrorCode = errorCode
            };
            return new JsonWithHttpCodeResult(response, HttpStatusCode.BadRequest);
        }

        public static JsonResult RespondUnhandledError(Object data, ErrorCode errorCode)
        {
            var response = new ErrorResponseObject
            {
                ResponseType = "error",
                Data = data,
                ErrorCode = errorCode
            };
            return new JsonWithHttpCodeResult(response, HttpStatusCode.InternalServerError);
        }
    }
}
