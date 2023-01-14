using DCReplace.Data;
using MEC;
using PlayerRoles;
using System;
using PluginAPI.Core.Attributes;
using System.Collections.Generic;
using System.Linq;
using PluginAPI.Enums;
using PluginAPI.Core.Interfaces;
using PlayerStatsSystem;
using PluginAPI.Core;

namespace DCReplace
{
    public class EventHandlers
    {
        public static Random SystemRandom { get; } = new Random();
        public static List<string> DisconnectedPlayers { get; set; } = new List<string>();

        /*[PluginEvent(ServerEventType.RagdollSpawn)]
        public bool RagdollSpawn(Player player, IRagdollRole ragdoll, DamageHandlerBase damageHandler)
            => Check(player);*/

        public static T Random<T>(IEnumerable<T> @enum)
            => @enum.ElementAt(SystemRandom.Next(@enum.Count()));

        public static IEnumerator<float> RespawnPlayer(Player player, IData data)
        {
            yield return Timing.WaitForSeconds(2f);
            data.Apply(player);
            DisconnectedPlayers.Remove(player.ReferenceHub.characterClassManager.UserId);
        }

        public bool Check(Player player)
            => DisconnectedPlayers.Contains(player.ReferenceHub.characterClassManager.UserId);
    }
}
