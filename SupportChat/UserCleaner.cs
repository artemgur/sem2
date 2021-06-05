using System;
using System.Threading.Tasks;

namespace SupportChat
{
    public partial class ChatHub
    {
        private class UserCleaner
        {
            private static UserCleaner Instance { get; set; }

            private readonly IChatDatabase database;

            private UserCleaner(IChatDatabase database)
            {
                this.database = database;
            }

            public static void StartIfNotStarted(IChatDatabase database)
            {
                if (Instance == null)
                {
                    Instance = new UserCleaner(database);
                    Instance.CleanInactiveUsers();
                }
            }

            private async Task CleanInactiveUsers()
            {
                while (true)
                {
                    await Task.Delay(10000);
                    foreach (var (key, value) in ChatHub.UserLastMessage)
                    {
                        if ((DateTime.Now - value).Seconds > UserTimeoutSeconds)
                            DeleteUserData(key, database);
                    }
                }
            }
        }
    }
}