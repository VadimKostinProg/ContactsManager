using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ExceptionFilters
{
    public class HandleExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<HandleExceptionFilter> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HandleExceptionFilter(ILogger<HandleExceptionFilter> logger,
            IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError("Exception filter {FilterName}.{ActionName}\n{ExceptionType}.{ExceptionMessage}",
                nameof(HandleExceptionFilter), nameof(OnException),
                context.Exception.ToString(), context.Exception.Message);

            if (_webHostEnvironment.IsDevelopment())
                context.Result = new ContentResult()
                {
                    Content = context.Exception.Message,
                    StatusCode = 500
                };
        }
    }
}
