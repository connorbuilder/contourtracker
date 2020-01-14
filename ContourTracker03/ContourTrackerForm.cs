using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ContourTracker03
{
    public partial class ContourTrackerForm : Form
    {
        private BusinessContour _businessContour;
        private string _fileName = null;
        private bool _isSmoothed = false;
        private List<ExtPoint> _allSmmothedLists;

        public ContourTrackerForm()
        {
            InitializeComponent();
            this._openFileButton.MouseDown += new MouseEventHandler(_openFileButton_MouseDown);
            this._setContourLevelButon.MouseDown += new MouseEventHandler(_setContourLevelButon_MouseDown);
            this._smoothButton.MouseDown += new MouseEventHandler(_smoothButton_MouseDown);
            this.panel1.Paint += new PaintEventHandler(panel1_Paint);
        }

        private void _setContourLevelButon_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (_businessContour == null)
                {
                    MessageBox.Show("没有数据！");
                    return;
                }

                ContourLevelsForm contourLevelsForm = new ContourLevelsForm();
                contourLevelsForm.ContourLevel = _businessContour.ContourLevel;

                if (DialogResult.OK == contourLevelsForm.ShowDialog())
                {
                    _businessContour.ContourLevel = contourLevelsForm.ContourLevel;
                    _businessContour.IsDefaultContourLevel = false;
                    _businessContour.BeginSeriesAction();
                    _isSmoothed = false;

                    this.panel1.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }

        private void _openFileButton_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = @"..\..\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|dat files (*.dat)|*.dat|All files (*.*)|*.*";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    _businessContour = new BusinessContour(openFileDialog.FileName);
                    _businessContour.IsDefaultContourLevel = true;
                    _businessContour.BeginSeriesAction();          //这里就开始得到等值线的一系列操作
                    _isSmoothed = false;
                    this.panel1.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void _smoothButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (_businessContour == null)
                return;

            SmoothContour.Clip = 4;
            _allSmmothedLists = SmoothContour.SmoothingContour(_businessContour.AllIsoPointList);
            _isSmoothed = true;
            this.panel1.Refresh();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (_businessContour == null)
                return;

            Graphics g = e.Graphics;
            PointF[] points;
            if (!_isSmoothed)
            {
                List<IsoPointListInfo> allList = _businessContour.AllIsoPointList;
                List<IsoPoint> aIsoList;
                
                for (int i = 0; i < allList.Count; i++)
                {
                    aIsoList = allList[i]._aIsoPointList;
                    points = new PointF[aIsoList.Count];

                    for (int j = 0; j < aIsoList.Count; j++)
                    {
                        points[j].X = aIsoList[j]._x;
                        points[j].Y = aIsoList[j]._y;
                    }
                    g.DrawLines(Pens.Red, points);
                }
            }
            else
            {
                List<PointF> aIsoList;
                for (int i = 0; i < _allSmmothedLists.Count; i++)
                {
                    aIsoList = _allSmmothedLists[i].contourList;
                    points = new PointF[aIsoList.Count];

                    for (int j = 0; j < aIsoList.Count; j++)
                    {
                        points[j].X = aIsoList[j].X;
                        points[j].Y = aIsoList[j].Y;
                    }

                    if (points.Length < 1)
                        continue;

                    g.DrawLines(Pens.Red, points);
                }
            }
        }
    }
}
