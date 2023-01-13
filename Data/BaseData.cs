using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCReplace.Data
{
    public abstract class BaseData
    {
        public BaseData(Player player)
            => Initialize(player);
        public abstract void Initialize(Player player);
        public abstract void Apply(Player player);
    }
}
