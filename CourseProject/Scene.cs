using ClassLibraryShapes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CourseProject
{
    public partial class Scene : Form, IGraphics
    {
        private List<Shape> _shapes = new List<Shape>();
        private Point _captureLocation;
        private bool _captureMouse;
        private Shape _frame;
        private Rectangle _rectangleFrame;
        private bool _rectangleFramePaint;
        private Graphics _onPaintGraphics;
        

        public Scene()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer,
                     true);
        }

        public void DrawRectangle(Color borderColor, Color? fillColor, int x, int y, int width, int height)
        {
            if (_onPaintGraphics != null)
            {
                if (fillColor.HasValue)
                {
                    using (var brush = new SolidBrush(fillColor.Value))
                    {
                        _onPaintGraphics.FillRectangle(brush, x, y, width, height);
                    }
                }

                using (Pen pen = new Pen(borderColor, 3))
                {
                    _onPaintGraphics.DrawRectangle(pen, x, y, width, height);
                }
            }
        }

        public void DrawEllipse(Color borderColor, Color? fillColor, int x, int y, int width, int height)
        {
            if (_onPaintGraphics != null)
            {
                if (fillColor.HasValue)
                {
                    using (var brush = new SolidBrush(fillColor.Value))
                    {
                        _onPaintGraphics.FillEllipse(brush, x, y, width, height);
                    }
                }

                using (Pen pen = new Pen(borderColor, 3))
                {
                    _onPaintGraphics.DrawEllipse(pen, x, y, width, height);
                }
            }
        }

        public void DrawTriangle(Color borderColor, Color? fillColor, Point[] points)
        {
            if (_onPaintGraphics != null)
            {
                if (fillColor.HasValue)
                {
                    using (var brush = new SolidBrush(fillColor.Value))
                    {
                        _onPaintGraphics.FillPolygon(brush, points);
                    }
                }
                using (Pen pen = new Pen(borderColor, 3))
                {
                    _onPaintGraphics.DrawPolygon(pen, points);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            _onPaintGraphics = e.Graphics;

            for(int i = _shapes.Count - 1; i >= 0; i--)
            {
                _shapes[i].Paint(this);
            }

            if(_captureMouse && _frame != null)
            {
                _frame.Paint(this);
            }

            if(_captureMouse && _rectangleFrame != null)
            {
                _rectangleFrame.Paint(this);
            }

            _onPaintGraphics = null;
        }

        private void Scene_MouseDown(object sender, MouseEventArgs e)
        {
            _captureMouse = true;
            _captureLocation = e.Location;
            _frame = null;
            _rectangleFrame = null;

            foreach(var shape in _shapes)
            {
                shape.Selected = false;
            }

            var selectedShapes = _shapes
                .FirstOrDefault(s => s.Contains(e.Location));

            if(selectedShapes != null)
            {
                selectedShapes.Selected = true;
            }

            Invalidate();
        }

        private void Scene_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_captureMouse)
            {
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                var selectedShape = _shapes
                    .FirstOrDefault(s => s.Selected == true);

                if (selectedShape != null && !_rectangleFramePaint)
                {
                    MoveShapes(selectedShape, e.Location);                   
                }
                else
                {
                    _rectangleFramePaint = true;

                    _rectangleFrame = CreateRectangleFrame(e.Location);
                    _rectangleFrame.Fill = false;
                    _rectangleFrame.Color = Color.LightGray;

                    foreach (var shape in _shapes
                        .Where(s => s.Intersects(_rectangleFrame)))
                    {
                        shape.Selected = true;
                    }

                    foreach (var shape in _shapes
                        .Where(s => !s.Intersects(_rectangleFrame)))
                    {
                        shape.Selected = false;
                    }
                }
            }
            else if(e.Button == MouseButtons.Right)
            {
                _frame = CreateFrame(e.Location);
                _frame.Fill = false;
                _frame.Color = Color.LightGray;
            }

            Invalidate();
        }

        private void Scene_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_captureMouse)
            {
                return;
            }

            if(e.Button == MouseButtons.Right && _frame != null)
            {
                _frame.Fill = true;
                _frame.Selected = true;
                _frame.Color = Color.Blue;
                _shapes.Insert(0, _frame);
            }
            else if(e.Button == MouseButtons.Left)
            {
                _rectangleFramePaint = false;
            }

            _captureMouse = false;

            Invalidate();
        }

        private void MoveShapes (Shape shape, Point location)
        {
            if(shape.Type == "Triangle")
            {
                Triangle triangle = (Triangle)shape;
                location.Y -= (triangle.points[1].Y - triangle.points[0].Y) / 2;

                triangle.points[2].X += location.X - triangle.points[0].X;
                triangle.points[2].Y += location.Y - triangle.points[0].Y;
                triangle.points[1].X += location.X - triangle.points[0].X;
                triangle.points[1].Y += location.Y - triangle.points[0].Y;
                triangle.points[0].X = location.X;
                triangle.points[0].Y = location.Y;                                   
            }
            else if(shape.Type == "Rectangle")
            {
                Rectangle rectangle = (Rectangle)shape;
                location.X -= rectangle.Width / 2;
                location.Y -= rectangle.Height / 2;

                rectangle.Location = new Point(
                    location.X,
                    location.Y);
            }
            else
            {
                Ellipse ellipse = (Ellipse)shape;
                location.X -= ellipse.Width / 2;
                location.Y -= ellipse.Height / 2;

                ellipse.Location = new Point(
                    location.X,
                    location.Y);
            }
        }

        private Rectangle CreateRectangleFrame(Point location)
        {
            Rectangle frame = new Rectangle
            {
                Location = new Point(
                    Math.Min(_captureLocation.X, location.X),
                    Math.Min(_captureLocation.Y, location.Y)),
                Width = Math.Abs(_captureLocation.X - location.X),
                Height = Math.Abs(_captureLocation.Y - location.Y)
            };
            return frame;
        }

        private Shape CreateFrame(Point location)
        {
            Shape frame;
            if (radioButtonTriangle.Checked)
            {
                if (_captureLocation.X <= location.X)
                {
                    frame = new Triangle
                    {
                        points = new Point[]
                        {
                        new Point(_captureLocation.X, _captureLocation.Y),
                        new Point(location.X, location.Y),
                        new Point(_captureLocation.X - Math.Abs(_captureLocation.X - location.X), location.Y)
                        }
                    };
                }
                else
                {
                    frame = new Triangle
                    {
                        points = new Point[]
                        {
                        new Point(_captureLocation.X, _captureLocation.Y),
                        new Point(location.X, location.Y),
                        new Point(_captureLocation.X + Math.Abs(_captureLocation.X - location.X), location.Y)
                        }
                    };
                }
            }
            else if (radioButtonEllipse.Checked)
            {
                frame = new Ellipse
                {
                    Location = new Point(
                        Math.Min(_captureLocation.X, location.X),
                        Math.Min(_captureLocation.Y, location.Y)),
                    Width = Math.Abs(_captureLocation.X - location.X),
                    Height = Math.Abs(_captureLocation.Y - location.Y)
                };
            }
            else
            {
                frame = new Rectangle
                {
                    Location = new Point(
                        Math.Min(_captureLocation.X, location.X),
                        Math.Min(_captureLocation.Y, location.Y)),
                    Width = Math.Abs(_captureLocation.X - location.X),
                    Height = Math.Abs(_captureLocation.Y - location.Y)
                };
            }

            return frame;
        }

        private void DeleteSelected()
        {
            for (int i = _shapes.Count - 1; i >= 0; i--)
            {
                if (_shapes[i].Selected)
                {
                    _shapes.RemoveAt(i);
                }
            }
            Invalidate();
        }

        private void Scene_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode != Keys.Delete)
            {
                return;
            }

            DeleteSelected();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DeleteSelected();
        }

        private void Scene_DoubleClick(object sender, EventArgs e)
        {
            var shape = _shapes
                .FirstOrDefault(s => s.Selected);

            if(shape != null)
            {
                if(shape.Type == "Triangle")
                {
                    Triangle triangle = (Triangle)shape;
                    var fc = new FormPropertiesTriangle();
                    fc.AX = triangle.points[0].X;
                    fc.AY = triangle.points[0].Y;
                    fc.BX = triangle.points[1].X;
                    fc.BY = triangle.points[1].Y;
                    fc.CX = triangle.points[2].X;
                    fc.CY = triangle.points[2].Y;
                    fc.Color = triangle.Color;

                    if(fc.ShowDialog() == DialogResult.OK)
                    {
                        triangle.points[0].X = fc.AX;
                        triangle.points[0].Y = fc.AY;
                        triangle.points[1].X = fc.BX;
                        triangle.points[1].Y = fc.BY;
                        triangle.points[2].X = fc.CX;
                        triangle.points[2].Y = fc.CY;
                        triangle.Color = fc.Color;
                    }
                }
                else if(shape.Type == "Rectangle")
                {
                    Rectangle rectangle = (Rectangle)shape;
                    var fc = new FormPropertiesRectangle();
                    fc.MyWidth = rectangle.Width;
                    fc.MyHeight = rectangle.Height;
                    fc.MyColor = rectangle.Color;

                    if (fc.ShowDialog() == DialogResult.OK)
                    {
                        rectangle.Width = fc.MyWidth;
                        rectangle.Height = fc.MyHeight;
                        rectangle.Color = fc.MyColor;
                    }
                }
                else
                {
                    Ellipse ellipse = (Ellipse)shape;
                    var fc = new FormPropertiesRectangle();
                    fc.MyWidth = ellipse.Width;
                    fc.MyHeight = ellipse.Height;
                    fc.MyColor = ellipse.Color;

                    if (fc.ShowDialog() == DialogResult.OK)
                    {
                        ellipse.Width = fc.MyWidth;
                        ellipse.Height = fc.MyHeight;
                        ellipse.Color = fc.MyColor;
                    }
                }
                Invalidate();
            }
        }

        private void buttonArea_Click(object sender, EventArgs e)
        {
            var selectedShapesArea = _shapes
                .Where(s => s.Selected)
                .Sum(s => s.CalculateArea());

            MessageBox.Show($"Shape Area = {selectedShapesArea}", "Shape Area", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            var formatter = new BinaryFormatter();
            var sfd = new SaveFileDialog();
            sfd.FileName = "data";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (var stream = new FileStream(sfd.FileName, FileMode.Create))
                {
                    formatter.Serialize(stream, _shapes);
                }
            }
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.FileName = "data";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var formatter = new BinaryFormatter();

                using (var stream = new FileStream(ofd.FileName, FileMode.Open))
                {
                    _shapes = (List<Shape>)formatter.Deserialize(stream);
                }
            }
            Invalidate();
        }
    }
}
