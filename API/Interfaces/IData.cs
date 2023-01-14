using PluginAPI.Core;

namespace DCReplace.Data
{
    public interface IData
    {
        void Initialize(Player player);
        void Apply(Player player);
    }
}
