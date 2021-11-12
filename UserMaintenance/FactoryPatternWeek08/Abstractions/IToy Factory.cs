using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryPatternWeek08.Abstractions
{
    public interface IToy_Factory
    {
        Toy CreateNew();
    }
}
