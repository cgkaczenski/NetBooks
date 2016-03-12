using System.Collections.Generic;

namespace BootstrapIntro.Models
{
    public static class AuthenticatedUsers
    {
        private static List<User> _users = new List<User>
        {
            new User("chris", "kaczenski", null, new List<string> { "::1"})
        };

        public static List<User> Users { get { return _users; } }
    }
}