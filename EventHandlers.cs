using DCReplace.Data;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DCReplace
{
    public class EventHandlers : IDisposable
    {
        public List<string> DisconnectedPlayers { get; set; } = new List<string>();

        public EventHandlers()
        {
            Exiled.Events.Handlers.Player.Left += OnLeft;
            Exiled.Events.Handlers.Player.SpawningRagdoll += OnSpawningRagdoll;
            Exiled.Events.Handlers.Map.AnnouncingScpTermination += OnAnnouncingScpTermination;
        }

        public void Dispose()
        {
            Exiled.Events.Handlers.Player.Left -= OnLeft;
            Exiled.Events.Handlers.Player.SpawningRagdoll -= OnSpawningRagdoll;
            Exiled.Events.Handlers.Map.AnnouncingScpTermination -= OnAnnouncingScpTermination;
        }

        public void OnLeft(LeftEventArgs ev)
        {
            var spectators = Player.Get(RoleTypeId.Spectator);
            if (spectators.IsEmpty() || Plugin.Instance.Config.BlacklistedRoles.Contains(ev.Player.Role.Type)) //TODO: replace "queue", if someone dies within specified time, have them replace him
                return;

            var player = Random(spectators);
            DisconnectedPlayers.Add(ev.Player.UserId);
            player.Role.Set(ev.Player.Role.Type);

            BaseData data;

            if (ev.Player.Role.Type == RoleTypeId.Scp079)
                data = new Scp079Data(ev.Player);
            else
                data = new FpcData(ev.Player);

            Timing.RunCoroutine(RespawnPlayer(player, data));
            DisconnectedPlayers.Add(ev.Player.UserId);
        }

        public void OnSpawningRagdoll(SpawningRagdollEventArgs ev)
        {
            if (Check(ev.Player))
                ev.IsAllowed = false;
        }

        public void OnAnnouncingScpTermination(AnnouncingScpTerminationEventArgs ev)
        {
            if (Check(ev.Player))
                ev.IsAllowed = false;
        }

        public T Random<T>(IEnumerable<T> @enum)
            => @enum.ElementAt(Exiled.Loader.Loader.Random.Next(0, @enum.Count()));

        public IEnumerator<float> RespawnPlayer(Player player, BaseData data)
        {
            yield return Timing.WaitForSeconds(0.5f);
            data.Apply(player);
            DisconnectedPlayers.Remove(player.UserId);
        }

        public bool Check(Player player)
            => DisconnectedPlayers.Contains(player.UserId);
    }
}
