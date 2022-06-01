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
    public class Ellipse : Shape
    {
        public Point Location { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public override string Type
        {
            get
            {
                return "Ellipse";
            }
        }

        public override void Paint(IGraphics g)
        {
            var borderedColor = Selected
                ? Color.Red
                : Color;

            g.DrawEllipse(borderedColor, Fill ? Color : (Color?)null, Location.X, Location.Y, Width, Height); 
        }

        public override bool Contains(Point p)
        {
            Point center = new Point(
                Location.X + (Width / 2),
                Location.Y + (Height / 2));

            Point normalized = new Point(
                p.X - center.X,
                p.Y - center.Y);

            double xRadius = Width / 2;
            double yRadius = Height / 2;

            return ((double)normalized.X * normalized.X) / (xRadius * xRadius) +
                   ((double)normalized.Y * normalized.Y) / (yRadius * yRadius) <= 1;
        }

        public override bool Intersects(Rectangle rectangle)
        {
            return
               this.Location.X < rectangle.Location.X + rectangle.Width &&
               rectangle.Location.X < this.Location.X + this.Width &&
               this.Location.Y < rectangle.Location.Y + rectangle.Height &&
               rectangle.Location.Y < this.Location.Y + this.Height;
        }

        public override double CalculateArea()
        {
            return
                Height * Width * Math.PI;
        }
    }
}
