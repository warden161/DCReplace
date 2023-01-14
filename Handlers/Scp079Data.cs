
using PlayerRoles.PlayableScps.Scp079;
using PlayerRoles.PlayableScps.Scp079.Cameras;
using PluginAPI.Core;

namespace DCReplace.Data
{
    public struct Scp079Data : IData
    {
        public Scp079Camera Camera { get; set; }
        public float Energy { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }

        public void Initialize(Player player)
        {
            GetData(player, out var role, out var auxManager, out var tierManager);

            Camera = role.CurrentCamera;
            Energy = auxManager.CurrentAux;
            Level = tierManager.AccessTierLevel;
            Experience = tierManager.TotalExp;
        }

        public void Apply(Player player)
        {
            GetData(player, out var role, out var auxManager, out var tierManager);

            Camera = role.CurrentCamera;
            Energy = auxManager.CurrentAux;
            Level = tierManager.AccessTierLevel;
            Experience = tierManager.TotalExp;
        }

        private void GetData(Player player, out Scp079Role role, out Scp079AuxManager auxManager, out Scp079TierManager tierManager)
        {
            role = player.ReferenceHub.roleManager.CurrentRole as Scp079Role;
            role.SubroutineModule.TryGetSubroutine(out auxManager);
            role.SubroutineModule.TryGetSubroutine(out tierManager);
        }
    }
}
