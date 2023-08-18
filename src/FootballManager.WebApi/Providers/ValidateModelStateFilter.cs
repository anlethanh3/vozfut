using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace FootballManager.WebApi.Providers
{
    public class ValidateModelStateFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Keys
                    .Where(k => context.ModelState[k].Errors.Count > 0)
                    .Select(k => new
                    {
                        Name = k,
                        Messages = context.ModelState[k].Errors
                    .Select(x => x.ErrorMessage).ToList()
                    })
                    .ToList();

                List<string> messages = new();
                foreach (var item in errors)
                {
                    messages.Add(string.Join(",", item.Messages.Select(x => x)));
                }

                var responseData = new
                {
                    StatusCode = 400,
                    Message = string.Join(",", messages)
                };

                context.Result = new JsonResult(responseData)
                {
                    StatusCode = 200
                };
            }
        }
    }
}
