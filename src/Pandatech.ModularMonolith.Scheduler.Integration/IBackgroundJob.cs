using System.Linq.Expressions;

namespace Pandatech.ModularMonolith.Scheduler.Integration;

public interface IBackgroundJob
{
   public void Enqueue<T>(Expression<Action<T>> methodCall);
   public void Schedule<T>(Expression<Action<T>> methodCall, TimeSpan delay);
   public void Schedule<T>(Expression<Action<T>> methodCall, DateTimeOffset enqueueAt);
}