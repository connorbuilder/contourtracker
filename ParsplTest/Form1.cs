using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ParsplTest
{
    public partial class Form1 : Form
    {
        Point[] _origPoint;
        private List<Point> _listPoints;
        private Point[] _points;

        public Form1()
        {
            InitializeComponent();
            this.Size = new Size(640, 480);
            this.BackColor = Color.White;

            _listPoints = new List<Point>();

            InitOrigPoint();

            DrawFreeLine();
        }

        private void DrawFreeLine()
        {
            int clip = 10;

            double t1, t2, t3, t, a, b, c, d, x, y;
            t = 0.6f / clip;

            for(int i=0;i<_origPoint.Length-3;i++)
                for (int j = 1; j < clip; j++)
                {
                    t1 = j * t;
                    t2 = t1 * t1;
                    t3 = t2 * t1;
                    a = 4.0 * t2 - t1 - 4.0 * t3;
                    b = 1.0 - 10.0 * t2 + 12.0 * t3;
                    c = t1 + 8.0 * t2 - 12.0 * t3;
                    d = 4.0 * t3 - 2.0 * t2;
                    x = a * _origPoint[i].X + b * _origPoint[i + 1].X + c * _origPoint[i + 2].X + d * _origPoint[i + 3].X;
                    y = a * _origPoint[i].Y + b * _origPoint[i + 1].Y + c * _origPoint[i + 2].Y + d * _origPoint[i + 3].Y;

                    _listPoints.Add(new Point((int)x, (int)y));
                }

            _points = new Point[_listPoints.Count];
            for (int i = 0; i < _listPoints.Count; i++)
            {
                _points[i] = _listPoints[i];
            }
        }

        private void InitOrigPoint()
        {
            _origPoint = new Point[11];
            _origPoint[0] = new Point(264, 100);
            _origPoint[1] = new Point(264, 144);
            _origPoint[2] = new Point(163, 114);
            _origPoint[3] = new Point(300, 88);
            _origPoint[4] = new Point(339, 213);
            _origPoint[5] = new Point(80, 223);
            _origPoint[6] = new Point(160, 36);
            _origPoint[7] = new Point(422, 37);
            _origPoint[8] = new Point(435, 246);
            _origPoint[9] = new Point(176, 293);
            _origPoint[10] = new Point(414, 335);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if(_points.Length!=0)
                e.Graphics.DrawLines(Pens.Black, _points);
            e.Graphics.DrawLines(Pens.Red, _origPoint);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            
        }
    }
}
