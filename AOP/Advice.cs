using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AOP
{
    public abstract class Advice<T> : DispatchProxy
    {
        protected T _decorated;
        private TaskScheduler _loggingScheduler;

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            if (targetMethod == null) throw new ArgumentException(nameof(targetMethod));
            
            Before(targetMethod, args);

            var result = Around(targetMethod, args);

            if (result is Task resultTask)
            {
                resultTask.ContinueWith(task =>
                    {
                        object taskResult = null;
                        if (task.GetType().GetTypeInfo().IsGenericType &&
                            task.GetType().GetGenericTypeDefinition() == typeof(Task<>))
                        {
                            var property = task.GetType().GetTypeInfo().GetProperties()
                                .FirstOrDefault(p => p.Name == "Result");
                            if (property != null)
                            {
                                taskResult = property.GetValue(task);
                            }
                        }
                
                        After(targetMethod, args, taskResult);
                    },
                    _loggingScheduler);
            }
            else
            {
                After(targetMethod, args, result);
            }

            return result;
        }

        
        internal void SetParameters(T decorated, TaskScheduler loggingScheduler)
        {
            if (decorated == null)
            {
                throw new ArgumentNullException(nameof(decorated));
            }

            _decorated = decorated;
            _loggingScheduler = loggingScheduler ?? TaskScheduler.FromCurrentSynchronizationContext();
        }

        protected abstract void After(MethodInfo methodInfo, object[] args, object result);

        protected abstract void Before(MethodInfo methodInfo, object[] args);
        
        protected abstract object Around(MethodInfo methodInfo, object[] args);
    }
}