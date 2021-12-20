using System;
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
            Console.WriteLine($"[{DateTime.Now}] ManagerJob Execute begin Ver." + Ver.ToString());
            try
            {
                await new QuartzManager().JobScheduler(context.Scheduler);
                Console.WriteLine($"[{DateTime.Now}] ManagerJob Executing ...");
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
            finally
            {
                Console.WriteLine($"[{DateTime.Now}] ManagerJob Execute end ");
            }
        }
    }
}