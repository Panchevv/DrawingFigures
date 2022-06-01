using ClassLibraryShapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject
{
    [Serializable]
    public class Rectangle : Shape
    {
        public Point Location { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public override string Type
        {
            get
            {
                return "Rectangle";
            }
        }

        public override void Paint(IGraphics g)
        {
            var borderedColor = Selected
                ? Color.Red
                : Color;

            g.DrawRectangle(borderedColor, Fill ? Color : (Color?)null, Location.X, Location.Y, Width, Height);

        }

        public override double CalculateArea()
        {
            return Width * Height;
        }

        public override bool Contains(Point p)
        {
            return
                Location.X < p.X && p.X < Location.X + Width &&
                Location.Y < p.Y && p.Y < Location.Y + Height;
        }

        public override bool Intersects(Rectangle rectangle)
        {
            return
                this.Location.X < rectangle.Location.X + rectangle.Width &&
                rectangle.Location.X < this.Location.X + this.Width &&
                this.Location.Y < rectangle.Location.Y + rectangle.Height &&
                rectangle.Location.Y < this.Location.Y + this.Height;
        }
    }
}
