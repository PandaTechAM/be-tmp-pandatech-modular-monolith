using System.Linq.Expressions;
using Hangfire;
using Pandatech.ModularMonolith.Scheduler.Integration;

namespace Pandatech.ModularMonolith.Scheduler.Services;

public class BackgroundJob(IBackgroundJobClient backgroundJobClient) : IBackgroundJob
{
   public void Enqueue<T>(Expression<Action<T>> methodCall)
   {
      backgroundJobClient.Enqueue(methodCall);
   }

   public void Schedule<T>(Expression<Action<T>> methodCall, TimeSpan delay)
   {
      backgroundJobClient.Schedule(methodCall, delay);
   }

   public void Schedule<T>(Expression<Action<T>> methodCall, DateTimeOffset enqueueAt)
   {
      backgroundJobClient.Schedule(methodCall, enqueueAt);
   }
}