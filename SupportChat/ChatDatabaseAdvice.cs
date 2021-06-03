using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SupportChat;

namespace AOP
{
    //Caches messages to make calls to the database rarer
    public class ChatDatabaseAdvice: Advice<IChatDatabase>
    {
        private static Dictionary<int, List<MessageDTO>> messageCache = new Dictionary<int, List<MessageDTO>>();
        
        protected override void After(MethodInfo methodInfo, object[] args, object result)
        {
            if (methodInfo.Name == "GetMessages")
            {
                var userId = (int)args[0];
                if (userId != -1 && !messageCache.ContainsKey(userId))
                {
                    messageCache[userId] = (List<MessageDTO>) result;
                }
            }
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
            if (methodInfo.Name == "GetMessages")
            {
                var userId = (int)args[0];
                if (userId != -1)
                {
                    if (messageCache.ContainsKey(userId))
                        return messageCache[userId];
                    //messageCache[userId] = (List<MessageDTO>) methodInfo.Invoke(_decorated, args); //Can't add so far, we need to wait to the end of task
                }
            }
            
            return methodInfo.Invoke(_decorated, args); //Just call method normally if nothing else returned so far
        }
    }
}