using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShortLink.Core.Exceptions;

namespace ShortLink.WebAPI.Filters
{
    public class CoreExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly Dictionary<Type, Func<IActionResult>> _resultFactories = new()
        {
            { typeof(NotFoundException), () => new NotFoundResult() },
        };
        
        public override void OnException(ExceptionContext context)
        {
            if (!_resultFactories.TryGetValue(context.Exception.GetType(), out var resultFactory)) 
                return;
            
            context.Result = resultFactory.Invoke();
            context.ExceptionHandled = true;
        }
    }
}