using Exiled.API.Interfaces;
using PlayerRoles;
using System.Collections.Generic;
using System.ComponentModel;

namespace DCReplace
{
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; }
        public string ReplaceMessage { get; set; } = "Replaced a player who disconnected";
        public List<RoleTypeId> BlacklistedRoles { get; set; } = new List<RoleTypeId>();
    }
}
