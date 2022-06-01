using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CourseProject
{
    public partial class FormPropertiesRectangle : Form
    {
        public FormPropertiesRectangle()
        {
            InitializeComponent();
        }

        private int _width;
        private int _height;
        private Color _color;

        public int MyWidth
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
                textBoxWidth.Text = _width.ToString();
            }
        }

        public int MyHeight
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
                textBoxHeight.Text = _height.ToString();
            }
        }
        public Color MyColor
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                buttonColor.BackColor = _color;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if(int.TryParse(textBoxWidth.Text, out int width))
            {
                _width = width;
            }
            if (int.TryParse(textBoxHeight.Text, out int height))
            {
                _height = height;
            }
            DialogResult = DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void buttonColor_Click(object sender, EventArgs e)
        {
            var cd = new ColorDialog();
            if(cd.ShowDialog() == DialogResult.OK)
            {
                MyColor = cd.Color;
            }
        }
    }
}
