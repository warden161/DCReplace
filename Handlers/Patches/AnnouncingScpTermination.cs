using HarmonyLib;

namespace DCReplace.Handlers.Patches
{
    [HarmonyPatch(typeof(NineTailedFoxAnnouncer), nameof(NineTailedFoxAnnouncer.AnnounceScpTermination))]
    public class AnnouncingScpTermination
    {
        public static bool Prefix(ReferenceHub scp)
            => !EventHandlers.DisconnectedPlayers.Contains(scp.characterClassManager.UserId);
    }
}
