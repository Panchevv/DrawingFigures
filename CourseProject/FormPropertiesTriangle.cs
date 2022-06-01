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
    public partial class FormPropertiesTriangle : Form
    {
        public FormPropertiesTriangle()
        {
            InitializeComponent();
        }

        private int _ax;
        private int _ay;
        private int _bx;
        private int _by;
        private int _cx;
        private int _cy;
        private Color _color;

        public int AX
        {
            get
            {
                return _ax;
            }
            set
            {
                _ax = value;
                textBoxAX.Text = _ax.ToString();
            }
        }

        public int AY
        {
            get
            {
                return _ay;
            }
            set
            {
                _ay = value;
                textBoxAY.Text = _ay.ToString();
            }
        }

        public int BX
        {
            get
            {
                return _bx;
            }
            set
            {
                _bx = value;
                textBoxBX.Text = _bx.ToString();
            }
        }

        public int BY
        {
            get
            {
                return _by;
            }
            set
            {
                _by = value;
                textBoxBY.Text = _by.ToString();
            }
        }

        public int CX
        {
            get
            {
                return _cx;
            }
            set
            {
                _cx = value;
                textBoxCX.Text = _cx.ToString();
            }
        }

        public int CY
        {
            get
            {
                return _cy;
            }
            set
            {
                _cy = value;
                textBoxCY.Text = _cy.ToString();
            }
        }

        public Color Color
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
            if (int.TryParse(textBoxAX.Text, out int ax))
            {
                _ax = ax;
            }
            if (int.TryParse(textBoxAY.Text, out int ay))
            {
                _ay = ay;
            }
            if (int.TryParse(textBoxBX.Text, out int bx))
            {
                _bx = bx;
            }
            if (int.TryParse(textBoxBY.Text, out int by))
            {
                _by = by;
            }
            if(int.TryParse(textBoxCX.Text, out int cx))
            {
                _cx = cx;
            }
            if (int.TryParse(textBoxCY.Text, out int cy))
            {
                _cy = cy;
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
            if (cd.ShowDialog() == DialogResult.OK)
            {
                Color = cd.Color;
            }
        }
    }
}
