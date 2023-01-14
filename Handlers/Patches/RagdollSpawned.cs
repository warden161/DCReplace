using HarmonyLib;
using PlayerRoles.Ragdolls;

namespace DCReplace.Handlers.Patches
{
    [HarmonyPatch(typeof(RagdollManager), nameof(RagdollManager.ServerSpawnRagdoll))]
    public class RagdollSpawned
    {
        public static bool Prefix(ReferenceHub owner)
            => !EventHandlers.DisconnectedPlayers.Contains(owner.characterClassManager.UserId);
    }
}
