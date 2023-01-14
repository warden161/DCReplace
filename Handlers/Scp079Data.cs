using Exiled.API.Features;
using Exiled.API.Features.Roles;
using System.Data;

namespace DCReplace.Data
{
    public struct Scp079Data : IData
    {
        public Camera Camera { get; set; }
        public float Energy { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }

        public void Initialize(Player player)
        {
            var role = player.Role.As<Scp079Role>();

            Camera = role.Camera;
            Energy = role.Energy;
            Level = role.Level;
            Experience = role.Experience;
        }

        public void Apply(Player player)
        {
            var role = player.Role.As<Scp079Role>();
            role.Camera = Camera;
            role.Energy = Energy;
            role.Level = Level;
            role.Experience = Experience;
        }
    }
}
