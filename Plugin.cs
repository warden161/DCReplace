using System;
using DCReplace.Handlers.Patches;
using FMODUnity;
using HarmonyLib;
using PluginAPI.Core.Attributes;
using PluginAPI.Events;

namespace DCReplace
{
    public class Plugin
    {
        public static Plugin Instance { get; private set; }
        public Harmony Harmony { get; set; }
        public const string HarmonyId = "warden161.scpsl.dcreplace";

        [PluginEntryPoint("DCReplace", "0.1.0", "Replaces disconnected players.", "warden161")]
        public void OnEnabled()
        {
            Harmony = new Harmony(HarmonyId);
            Harmony.PatchAll();

            Instance = this;
            EventManager.RegisterEvents<EventHandlers>(this);
        }

        [PluginConfig] public Config Config;
    }
}
