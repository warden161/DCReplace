using PlayerRoles;
using System.Collections.Generic;
using System.ComponentModel;

namespace DCReplace
{
    public class Config
    {
        public string ReplaceMessage { get; set; } = "Replaced a player who disconnected";
        public List<RoleTypeId> BlacklistedRoles { get; set; } = new List<RoleTypeId>();
    }
}
