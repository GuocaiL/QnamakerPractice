using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Welcome_View;
using Microsoft.Bot.Builder;
using MyController_middleware_AdministratorSetings;

namespace MyController_timer_GreetingTimer
{   
    public class GreetingTimerJob : IJob
    {
        async Task IJob.Execute(IJobExecutionContext context)
        {
            WelcomBot bot = new WelcomBot(context.MergedJobDataMap.GetString("coversationid"));            
            await bot.Resume1();
        }
    }
    public class CheckAddNumber : IJob
    {
        async Task IJob.Execute(IJobExecutionContext context)
        {
            WelcomBot bot = new WelcomBot(context.MergedJobDataMap.GetString("coversationid"));

            await bot.Resume2();
                    
            throw new NotImplementedException();
        }
    }

    public static class Program
    {
        public static NameValueCollection props = new NameValueCollection
                {
                    { "quartz.serializer.type","binary" }
                };

        public static StdSchedulerFactory factory = new StdSchedulerFactory(props);
        public static IScheduler scheduler;
        public static ITurnContext context;
       


        public static async Task RunProgram()
        {
            try
            {
                // Grab the Scheduler instance from the Factory  
                scheduler = await factory.GetScheduler();
                // 启动任务调度器  
                await scheduler.Start();
            }
            catch (SchedulerException se)
            {
                await Console.Error.WriteLineAsync(se.ToString());
            }
        }

        public static async Task AddGreetingJob(string coversationid)
        {
            //var admin = Program.context.GetConversationState<ControllerAdministratorSetings>().Administrator;
            var admin = ControllerAdministratorSetings.Administrator;
            var timestart = admin[coversationid].settime.UtcDateTime;
            var Interval = admin[coversationid].IntervalInSeconds;
            try
            {
               var greetingjob = JobBuilder.Create<GreetingTimerJob>()
                   .WithIdentity("GreetingJob",coversationid)
                   .UsingJobData("coversationid", coversationid)                 
                   .Build();

               var greetingtrigger = (ISimpleTrigger)TriggerBuilder.Create()
                    .WithIdentity("GreetingJob", coversationid) // 给任务一个名字  
                    .StartAt(timestart) // 设置任务开始时间  
                    .ForJob("GreetingJob",coversationid) //给任务指定一个分组  
                    .WithSimpleSchedule(x=> x
                    .WithIntervalInSeconds(Interval)//循环的时间1秒1次 
                    .RepeatForever())
                    .Build();

                // 等待执行任务  
                await scheduler.ScheduleJob(greetingjob,greetingtrigger);
                // some sleep to show what's happening  
                //await Task.Delay(TimeSpan.FromMilliseconds(2000));
            }
            catch (SchedulerException se)
            {
                await Console.Error.WriteLineAsync(se.ToString());
            }

        }

        public static async Task DeleteGreetingJob(string coversationid)
        {
            try
            {
                //IScheduler sched = await factory.GetScheduler();;
                await scheduler.PauseTrigger(new TriggerKey("GreetingJob", coversationid));//停止触发器
                await scheduler.UnscheduleJob(new TriggerKey("GreetingJob", coversationid));//移除触发器
                await scheduler.DeleteJob(new JobKey("GreetingJob", coversationid));//删除任务
            }
            catch (SchedulerException se)
            {
                await Console.Error.WriteLineAsync(se.ToString());
            }
        }

        public static async Task Addwelcome15Job(string coversationid)
        {
            try
            {
              var  welcome15job = JobBuilder.Create<CheckAddNumber>()
                    .WithIdentity("welcome15", "coversationid")
                    .UsingJobData("coversationid", coversationid)
                    .Build();

              var  welcome15trigger = (ISimpleTrigger)TriggerBuilder.Create()
                    .WithIdentity("welcome15", "coversationid") // 给任务一个名字  
                    .StartAt(DateTime.Now.AddSeconds(900000)) // 设置任务开始时间  
                    .ForJob("welcome15", "coversationid") //给任务指定一个分组  
                    .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(0)  //循环的时间 1秒1次 
                    .WithRepeatCount(0))
                    .Build();

                // 等待执行任务  
                await scheduler.ScheduleJob(welcome15job, welcome15trigger);
                // some sleep to show what's happening  
                //await Task.Delay(TimeSpan.FromMilliseconds(2000));
            }
            catch (SchedulerException se)
            {
                await Console.Error.WriteLineAsync(se.ToString());
            }
        }

        public static async Task Deletewelcome15Job(string conversationid)
        {
            try
            {
                //IScheduler sched = await factory.GetScheduler();;
                await scheduler.PauseTrigger(new TriggerKey("welcome15", "coversationid"));//停止触发器
                await scheduler.UnscheduleJob(new TriggerKey("welcome15", "coversationid"));//移除触发器
                await scheduler.DeleteJob(new JobKey("welcome15", "coversationid"));//删除任务
            }
            catch (SchedulerException se)
            {
                await Console.Error.WriteLineAsync(se.ToString());
            }
        }
    }
}
