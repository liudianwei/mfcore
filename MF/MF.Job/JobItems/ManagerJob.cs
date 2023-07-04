using System;
using System.Diagnostics;
using System.Threading.Tasks;

using MF.Job.Core;

using Quartz;

namespace MF.Job.JobItems
{
    [DisallowConcurrentExecution]
    public class ManagerJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Version Ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            Stopwatch sw_qs = new Stopwatch();
            sw_qs.Start();//开始计时
            try
            {
                await new QuartzManager().JobScheduler(context.Scheduler);
            }
            catch (Exception ex)
            {
                _ = new JobExecutionException(ex)
                {
                    RefireImmediately = true
                };

                var c = Console.BackgroundColor;
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.BackgroundColor = c;
            }
            sw_qs.Stop();//结束计时
            Console.WriteLine($"[{DateTime.Now}] ManagerJob Execute Complete Ver.{Ver.ToString()},Time consuming {sw_qs.ElapsedMilliseconds}ms");
        }
    }
}