using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCReplace.Data
{
    public interface IData
    {
        void Initialize(Player player);
        void Apply(Player player);
    }
}
