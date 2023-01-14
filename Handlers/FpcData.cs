using Exiled.API.Features;
using Exiled.API.Features.Items;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DCReplace.Data
{
    public struct FpcData : IData
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }
        public float Health { get; set; }
        public float Ahp { get; set; }
        public List<Item> Items { get; set; }

        public void Initialize(Player player)
        {
            Position = player.Position;
            Rotation = player.Rotation;
            Scale = player.Scale;
            Health = player.Health;
            Ahp = player.ArtificialHealth;
            Items = player.Items.ToList();
        }

        public void Apply(Player player)
        {
            player.Position = Position;
            player.Rotation = Rotation;
            player.Scale = Scale;
            player.Health = Health;
            player.ArtificialHealth = Ahp;
            player.ResetInventory(Items);
        }
    }
}
