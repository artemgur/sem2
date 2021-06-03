using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Authentication.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Infrastructure
{
    public class AuthenticationServiceOptions
    {
        public string ConnectionString { get; set; }
        private List<string> _roles = new List<string>();
        private List<UserBuilder> _users = new List<UserBuilder>();
        internal IEnumerable<string> Roles => _roles;
        internal IEnumerable<UserBuilder> Users => _users;
        public string DefaultRole = null;

        public AuthenticationServiceOptions AddRole(string roleName)
        {
            _roles.Add(roleName);
            return this;
        }

        public AuthenticationServiceOptions AddUser(Action<UserBuilder> userBuilder)
        {
            var userBuilderObj = new UserBuilder();
            userBuilder(userBuilderObj);
            _users.Add(userBuilderObj);
            return this;
        }
    }

    public class UserBuilder
    {
        private Func<int, UserBuilder, IServiceProvider, Task> _buildHandler;
        public string Email = "";
        public string Password = "";
        private List<string> _roles = new List<string>();
        public Dictionary<string, string> AdditionalInfo = new Dictionary<string, string>();
        internal IEnumerable<string> Roles => _roles;

        public void AddRole(string roleName)
        {
            _roles.Add(roleName);
        }

        public void ContinueWith(Func<int, UserBuilder, IServiceProvider, Task> buildHandler)
        {
            _buildHandler += buildHandler;
        }

        internal async Task ExternalBuild(int id, IServiceProvider serviceProvider)
        {
            if(_buildHandler != null)
                await _buildHandler.Invoke(id, this, serviceProvider);
        }
    }
}