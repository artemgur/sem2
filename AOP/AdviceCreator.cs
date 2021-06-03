using System.Reflection;
using System.Threading.Tasks;

namespace AOP
{
    public static class AdviceCreator
    {
        public static T Create<T, TAdviceType>(T decorated, TaskScheduler scheduler = null)
            where TAdviceType: Advice<T>
        {
            object proxy = DispatchProxy.Create<T, TAdviceType>();
            ((TAdviceType) proxy).SetParameters(decorated, scheduler);

            return (T) proxy;
        }    }
}