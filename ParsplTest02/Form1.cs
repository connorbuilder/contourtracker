using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ParsplTest02
{
    public partial class Form1 : Form
    {
        private List<PointF> _origListPoint;
        private List<PointF> _interListPoint;
        private PointF[] _interPoints;

        private bool _isDrawing = false;

        public Form1()
        {
            InitializeComponent();
            this.BackColor = Color.White;
            this.Size = new Size(640, 480);
            this.Text = "曲线光滑";
            
            _origListPoint = new List<PointF>();
            _interListPoint = new List<PointF>();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (_isDrawing)
            {
                _origListPoint.Clear();
                _interListPoint.Clear();
                _isDrawing = false;
            }

            _origListPoint.Add(new PointF(e.X, e.Y));
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (_origListPoint.Count < 2)
                    return;

                DrawFreeLine();
                this.Refresh();
                _isDrawing = true;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_origListPoint.Count > 1)
            {   
                Graphics g = e.Graphics;

                g.DrawLines(Pens.Red, _interPoints);
                //for (int i = 0; i < _interListPoint.Count; i++)
                //{
                //    g.DrawEllipse(Pens.Blue, new RectangleF(_interListPoint[i].X - 3, _interListPoint[i].Y - 3, 6, 6));
                //}

                for (int i = 1; i < _origListPoint.Count - 1; i++)
                {
                    g.DrawEllipse(Pens.Black, new RectangleF(_origListPoint[i].X - 3, _origListPoint[i].Y - 3, 6, 6));
                }
            }
        }

        private void DrawFreeLine()
        {
            float interX = _origListPoint[1].X - _origListPoint[0].X;
            float interY = _origListPoint[1].Y - _origListPoint[0].Y;
            //_origListPoint.Insert(0, new PointF(_origListPoint[0].X - interX, _origListPoint[0].Y - interY));

            //interX = _origListPoint[_origListPoint.Count - 1].X - _origListPoint[_origListPoint.Count - 2].X;
            //interY = _origListPoint[_origListPoint.Count - 1].Y - _origListPoint[_origListPoint.Count - 2].Y;
            //_origListPoint.Add(new PointF(_origListPoint[_origListPoint.Count - 1].X + interX, _origListPoint[_origListPoint.Count - 1].Y + interY));
            
            _origListPoint.Add(new PointF(_origListPoint[0].X, _origListPoint[0].Y));
            _origListPoint.Add(new PointF(_origListPoint[1].X, _origListPoint[1].Y));

            _origListPoint.Insert(0, new PointF(_origListPoint[_origListPoint.Count - 3].X, _origListPoint[_origListPoint.Count - 3].Y));

            //interX = _origListPoint[_origListPoint.Count - 1].X - _origListPoint[_origListPoint.Count - 2].X;
            //interY = _origListPoint[_origListPoint.Count - 1].Y - _origListPoint[_origListPoint.Count - 2].Y;
            //_origListPoint.Add(new PointF(_origListPoint[_origListPoint.Count - 1].X + interX, _origListPoint[_origListPoint.Count - 1].Y + interY));

            int clip = 10;

            double t1, t2, t3, t, a, b, c, d, x, y;
            t = 0.6f / clip;

            for (int i = 0; i < _origListPoint.Count - 3; i++)
                for (int j = 1; j < clip; j++)
                {
                    t1 = j * t;
                    t2 = t1 * t1;
                    t3 = t2 * t1;
                    a = 4.0 * t2 - t1 - 4.0 * t3;
                    b = 1.0 - 10.0 * t2 + 12.0 * t3;
                    c = t1 + 8.0 * t2 - 12.0 * t3;
                    d = 4.0 * t3 - 2.0 * t2;
                    x = a * _origListPoint[i].X + b * _origListPoint[i + 1].X + c * _origListPoint[i + 2].X + d * _origListPoint[i + 3].X;
                    y = a * _origListPoint[i].Y + b * _origListPoint[i + 1].Y + c * _origListPoint[i + 2].Y + d * _origListPoint[i + 3].Y;

                    _interListPoint.Add(new PointF((float)x, (float)y));
                }
            _interListPoint.Insert(0, _origListPoint[1]);
            _interListPoint[_interListPoint.Count - 1] = _origListPoint[_origListPoint.Count - 2];

            _interPoints = new PointF[_interListPoint.Count];
            for (int i = 0; i < _interListPoint.Count; i++)
            {
                _interPoints[i] = _interListPoint[i];
            }
            //
        }
    }
}
