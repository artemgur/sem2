using System.Collections.Generic;

namespace Authentication.Infrastructure
{
    public class AuthenticationServiceOptions
    {
        public string ConnectionString { get; set; }
        private List<string> _roles = new List<string>();
        public IEnumerable<string> Roles => _roles;
        public string DefaultRole = null;

        public AuthenticationServiceOptions AddRole(string roleName)
        {
            _roles.Add(roleName);
            return this;
        }
    }
}