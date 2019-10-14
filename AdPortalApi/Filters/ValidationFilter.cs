using System.Linq;
using System.Threading.Tasks;
using Dto.Contracts.ErrorContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AdPortalApi.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(ms => ms.Value.Errors.Count > 0)
                    .Select(ms => new ErrorModel
                    {
                        FieldName = ms.Key,
                        Messages = ms.Value.Errors.Select(e => e.ErrorMessage).ToList()
                    }).ToList();
                var errorResponse = new ErrorResponse {Errors = errors};
                context.Result = new BadRequestObjectResult(errorResponse);
                return;
            }

            await next();
        }
    }
}