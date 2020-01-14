using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ContourTracker02
{
    public partial class ContourTrackerForm : Form
    {
        private BusinessContour _businessContour;

        public ContourTrackerForm()
        {
            InitializeComponent();
            this._openFileButton.MouseDown += new MouseEventHandler(_openFileButton_MouseDown);
            this._setContourLevelButon.MouseDown += new MouseEventHandler(_setContourLevelButon_MouseDown);

            _businessContour = new BusinessContour();
        }

        private void _setContourLevelButon_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (_businessContour.FileName == null)
                {
                    MessageBox.Show("没有数据！");
                    return;
                }

                ContourLevelsForm contourLevelsForm = new ContourLevelsForm();
                contourLevelsForm.ContourLevel = _businessContour.ContourLevel;

                if (DialogResult.OK == contourLevelsForm.ShowDialog())
                {
                    _businessContour.ContourLevel = contourLevelsForm.ContourLevel;
                    _businessContour.BeginSeriesAction();
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
                    _businessContour.FileName = openFileDialog.FileName;
                    _businessContour.BeginSeriesAction();          //这里就开始得到等值线的一系列操作
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }
    }
}
