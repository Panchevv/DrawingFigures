using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ClassLibraryShapes;

namespace CourseProject
{
    [Serializable]
    public abstract class Shape
    {
        public Color Color { get; set; }
        public bool Fill { get; set; } = true;
        public bool Selected { get; set; }
        public abstract string Type { get;}

        public abstract void Paint(IGraphics g);

        public abstract double CalculateArea();

        public abstract bool Contains(Point p);

        public abstract bool Intersects(Rectangle rectangle);
    }
}
