using Dto.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var response = new ErrorResponse
            {
                Message = context.Exception.Message,
                Type = context.Exception.GetType().Name
            };

            context.Result = new ObjectResult(response);
        }
    }
}