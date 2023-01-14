using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Usables.Scp330;
using MEC;
using Mirror;
using PluginAPI.Core;
using PluginAPI.Core.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using YamlDotNet.Core.Tokens;

namespace DCReplace.Data
{
    public struct FpcData : IData
    {
        public Vector3 Position { get; set; }
        public Vector3 Scale { get; set; }
        public float Health { get; set; }
        public float Ahp { get; set; }
        public List<ItemBase> Items { get; set; }
        public Dictionary<ItemType, ushort> Ammo { get; set; }

        public void Initialize(Player player)
        {
            Position = player.Position;
            Scale = player.ReferenceHub.transform.localScale;
            Health = player.Health;
            Ahp = player.ArtificialHealth;
            Items = player.ReferenceHub.inventory.UserInventory.Items.Values.ToList();
            Ammo = player.ReferenceHub.inventory.UserInventory.ReserveAmmo;
        }

        public void Apply(Player player)
        {
            player.Position = Position;
            SetPlayerScale(player, Scale);
            player.Health = Health;
            player.ArtificialHealth = Ahp;
            ResetInventory(player);
        }

        // Player scale
        private MethodInfo _spawnMessage;
        public MethodInfo SendSpawnMessage => _spawnMessage ??= typeof(NetworkServer).GetMethod("SendSpawnMessage", BindingFlags.NonPublic | BindingFlags.Static);

        // https://github.com/Exiled-Team/EXILED/blob/master/Exiled.API/Features/Player.cs
        private void SetPlayerScale(Player player, Vector3 scale)
        {
            try
            {
                player.ReferenceHub.transform.localScale = scale;

                foreach (Player target in Player.GetPlayers())
                    SendSpawnMessage?.Invoke(null, new object[] { player.ReferenceHub.networkIdentity, target.Connection });
            }
            catch (Exception exception)
            {
                Log.Error($"{nameof(Scale)} error: {exception}");
            }
        }

        private void ResetInventory(Player player)
        {
            foreach (var invItem in Items)
                AddItem(player, invItem);

            foreach (var ammo in Ammo)
                player.ReferenceHub.inventory.ServerAddAmmo(ammo.Key, ammo.Value);
        }

        private void AddItem(Player player, ItemBase itemBase)
        {
            player.ReferenceHub.inventory.UserInventory.Items[itemBase.ItemSerial] = itemBase;

            itemBase.OnRemoved(null);
            itemBase.Owner = player.ReferenceHub;
            itemBase.OnAdded(null);

            if (player.ReferenceHub.inventory.isLocalPlayer && itemBase is IAcquisitionConfirmationTrigger acquisitionConfirmationTrigger)
            {
                acquisitionConfirmationTrigger.ServerConfirmAcqusition();
                acquisitionConfirmationTrigger.AcquisitionAlreadyReceived = true;
            }

            Timing.CallDelayed(0.02f, () =>
            {
                if (itemBase is Scp330Bag bag)
                    bag.ServerRefreshBag();
            });

            player.ReferenceHub.inventory.SendItemsNextFrame = true;
        }
    }
}
