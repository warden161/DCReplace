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
            var spectators = Player.List.Where(x => x.Role.IsDead);
            if (spectators.Count() == 0 || Plugin.Instance.Config.BlacklistedRoles.Contains(ev.Player.Role.Type)) //TODO: replace "queue", if someone dies within specified time, have them replace him
                return;

            var player = Random(spectators);
            DisconnectedPlayers.Add(ev.Player.UserId);
            player.Role.Set(ev.Player.Role.Type);

            IData data;
            if (ev.Player.Role.Type == RoleTypeId.Scp079)
                data = new Scp079Data();
            else
                data = new FpcData();

            data.Initialize(ev.Player);
            Timing.RunCoroutine(RespawnPlayer(player, data));
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
            => @enum.ElementAt(Exiled.Loader.Loader.Random.Next(@enum.Count()));

        public IEnumerator<float> RespawnPlayer(Player player, IData data)
        {
            yield return Timing.WaitForSeconds(0.5f);
            data.Apply(player);
            player.Broadcast(5, Plugin.Instance.Config.ReplaceMessage);
            DisconnectedPlayers.Remove(player.UserId);
        }

        public bool Check(Player player)
            => DisconnectedPlayers.Contains(player.UserId);
    }
}
