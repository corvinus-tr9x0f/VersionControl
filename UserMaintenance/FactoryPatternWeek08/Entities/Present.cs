using FactoryPatternWeek08.Abstractions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryPatternWeek08.Entities
{
    public class Present : Toy
    {
        public SolidBrush PresentColorBox { get; private set; }
        public SolidBrush PresentColorRibbon { get; private set; }

        public Present(Color colorBox, Color colorRibbon)
        {
            PresentColorBox = new SolidBrush(colorBox);
            PresentColorRibbon = new SolidBrush(colorRibbon);
        }

        protected override void DrawImage(Graphics g)
        {
            g.FillRectangle(PresentColorBox, 0, 0, Width, Height);
            g.FillRectangle(PresentColorRibbon, 0, (Height/5)*2, Width, Height/5);
            g.FillRectangle(PresentColorRibbon, (Width/5)*2, 0, Width/5, Height);
        }
    }    
}
