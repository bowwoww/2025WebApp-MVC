using Microsoft.AspNetCore.Mvc.Filters;

namespace Hotel.Filter
{
    public class LogFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Log the action execution start
            var agent = context.HttpContext.Request.Headers["User-Agent"].ToString();
            var ip = context.HttpContext.Connection.RemoteIpAddress?.ToString();
            var controller = context.Controller.GetType().Name;
            var action = context.RouteData.Values["action"];
            var timestamp = DateTime.Now;
            var logMessage = $"Action {action} in controller {controller} started at {timestamp} from IP {ip} with User-Agent {agent}";
            // You can also log to a file or a logging service instead of the console
            var filePath = "LogFiles/log.txt";
            if (!Directory.Exists("LogFiles"))
            {
                Directory.CreateDirectory("LogFiles");
            }

            using (var writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(logMessage);
            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Log the action execution end
            Console.WriteLine($"Action {context.ActionDescriptor.DisplayName} has finished at {DateTime.Now}");
        }
    }
}
