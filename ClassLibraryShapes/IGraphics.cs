using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryShapes
{
    public interface IGraphics
    {
        void DrawRectangle(Color borderColor, Color? fillColor, int x, int y, int width, int height);

        void DrawEllipse(Color borderColor, Color? fillColor, int x, int y, int width, int height);

        void DrawTriangle(Color borderColor, Color? fillColor, Point[] points);
    }
}
