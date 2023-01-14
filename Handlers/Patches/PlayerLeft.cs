using DCReplace.Data;
using HarmonyLib;
using MEC;
using Mirror;
using System;
using PlayerRoles;
using PluginAPI.Core;
using System.Linq;

namespace DCReplace.Handlers.Patches
{
    // I need this because this has to be called before destroyed
    [HarmonyPatch(typeof(CustomNetworkManager), nameof(CustomNetworkManager.OnServerDisconnect))]
    public class PlayerLeft
    {
        public static void Prefix(NetworkConnection conn)
        {
            try
            {
                if (conn.identity == null)
                    return;

                if (!ReferenceHub.TryGetHubNetID(conn.identity.netId, out ReferenceHub referenceHub))
                    return;

                var player = Player.Get(referenceHub);
                IData data;
                if (player.Role == RoleTypeId.Scp079)
                    data = new Scp079Data();
                else
                    data = new FpcData();

                var spectators = Player.GetPlayers().Where(x => x.Role == RoleTypeId.Spectator);
                if (spectators.Count() == 0 || Plugin.Instance.Config.BlacklistedRoles.Contains(player.Role)) //TODO: replace "queue", if someone dies within specified time, have them replace him
                    return;

                player.SetRole(RoleTypeId.Spectator);
                var selectedPlayer = EventHandlers.Random(spectators);
                EventHandlers.DisconnectedPlayers.Add(player.ReferenceHub.characterClassManager.UserId);
                selectedPlayer.SetRole(player.Role);

                data.Initialize(player);
                Timing.RunCoroutine(EventHandlers.RespawnPlayer(selectedPlayer, data));
            } catch (Exception e)
            {
                Log.Error(e.ToString());
            }
        }
    }
}
