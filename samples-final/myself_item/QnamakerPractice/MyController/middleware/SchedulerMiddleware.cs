using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using MyController_timer_GreetingTimer;

namespace MyController_middleware_SchedulerMiddleware
{
    public class SchedulerMiddleware : IMiddleware
    {
        async Task IMiddleware.OnTurn(ITurnContext context,
            MiddlewareSet.NextDelegate next)
        {           
                try
                {
                Program.context=context;
                Program.RunProgram().GetAwaiter().GetResult ();               
                await next();
                }
                catch (Exception e) { }            
        }
    }
}
