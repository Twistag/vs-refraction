using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refraction
{
    internal class UserCredentials
    {

        // Constructor
        public UserCredentials(string? userId, string? teamId)
        {
            this.UserId = userId;
            this.TeamId = teamId;
        }

        // Property for userId
        public string? UserId { get; }

        // Property for teamId
        public string? TeamId { get; }
    }
}
