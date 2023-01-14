using System;

namespace DCReplace
{
    public class Plugin : Exiled.API.Features.Plugin<Config>
    {
        public override string Name { get; } = "DCReplace";
        public override string Author { get; } = "warden161";
        public override Version Version { get; } = new Version(1, 1, 0);
        public override Version RequiredExiledVersion { get; } = new Version(6, 0, 0);

        public static Plugin Instance { get; private set; }
        internal EventHandlers Events { get; set; }

        public override void OnEnabled()
        {
            Instance = this;

            Events = new EventHandlers();
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Events.Dispose();

            Events = null;
            base.OnDisabled();
        }
    }
}
