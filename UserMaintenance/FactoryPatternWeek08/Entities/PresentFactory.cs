using FactoryPatternWeek08.Abstractions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryPatternWeek08.Entities
{
    public class PresentFactory : IToyFactory
    {
        public Color colorBox { get; set; }
        public Color colorRibbon { get; set; }
        public Toy CreateNew()
        {
            return new Present(colorBox, colorRibbon);
        }
    }
}
