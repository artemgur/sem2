using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SupportChat;

namespace AOP
{
    public class ChatDatabaseAdvice: Advice<IChatDatabase>
    {
        private static Dictionary<int, IOrderedEnumerable<MessageDTO>> messageCache = new ();
        
        protected override void After(MethodInfo methodInfo, object[] args, object result)
        {
            //Do nothing
        }

        protected override void Before(MethodInfo methodInfo, object[] args)
        {
            if (methodInfo.Name is "AddMessage" or "RemoveAllUserMessages") //Methods which change state of database, cache becomes outdated
            {
                var userId = (int)args[0];
                messageCache.Remove(userId);
            }
        }

        protected override object Around(MethodInfo methodInfo, object[] args)
        {
            if (methodInfo.Name == "AddMessage")
            {
                var userId = (int)args[0];
                if (messageCache.ContainsKey(userId))
                    return messageCache[userId];
            }
            
            return methodInfo.Invoke(_decorated, args); //Just call method normally if nothing else returned so far
        }
    }
}