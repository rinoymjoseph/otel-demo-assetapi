using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Otel.Demo.AssetApi.Filters
{
    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.All)]
    public sealed class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }
        public override Task OnExceptionAsync(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Result = new JsonResult(context.Exception.Message);
            _logger.LogError(context.Exception.Message);
            _logger.LogError(context.Exception.StackTrace);
            return base.OnExceptionAsync(context);
        }
    }
}
