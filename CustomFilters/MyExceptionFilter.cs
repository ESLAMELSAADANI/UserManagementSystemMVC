using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Day06_Demo.CustomFilters
{
    public class MyExceptionFilter:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine($"Action Method {context.ActionDescriptor.DisplayName} executing at  {DateTime.Now}");
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
             if(context.Exception != null)//Mean There Is Exception Occured Inside Action Method That This Attribute on it
            {
                context.ExceptionHandled = true;
                context.Result = new ContentResult() { Content = "Some Errors Occured, We Work On It" };
            }
        }
    }

   
}
