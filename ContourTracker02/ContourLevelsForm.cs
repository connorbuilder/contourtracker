using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace ContourTracker02
{
    public partial class ContourLevelsForm : Form
    {
        private float _zMin = 0;
        private float _zMax = 0;

        private float[] _contourLevel = null;

        private DialogResult _dialogResult;
        
        public ContourLevelsForm()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
        }

        #region 事件响应
        private void _okButton_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _contourLevel = new float[_contourListBox.Items.Count];

            for (int i = 0; i < _contourLevel.Length; i++)
            {
                _contourLevel[i] = (float)_contourListBox.Items[i];
            }

            _dialogResult = DialogResult.OK;
            this.Close();
        }

        private void _cancelButton_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _dialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void _deleteButton_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            int index = _contourListBox.SelectedIndex;
            
            if (index < 0)
                return;

            _contourListBox.Items.RemoveAt(index);
            if (_contourListBox.Items.Count <= index)
                _contourListBox.SelectedIndex = _contourListBox.Items.Count - 1;
            else
                _contourListBox.SelectedIndex = index;
        }

        private void _addButton_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            float addNum;
            if (!float.TryParse(_addTextBox.Text, out addNum))
            {
                _addTextBox.SelectAll();
                this.ActiveControl = _addTextBox;       //这个不要，就不可以达到选中的目的
                MessageBox.Show("输入数据错误");
                return;
            }
            else
            {
                if (addNum < _zMin || addNum > _zMax)
                {
                    _addTextBox.SelectAll();
                    this.ActiveControl = _addTextBox;       //这个不要，就不可以达到选中的目的
                    MessageBox.Show("输入数据错误（添加数据范围错误）");
                    return;
                }

                for (int i = 0; i < _contourListBox.Items.Count; i++)
                {
                    if ((float)(_contourListBox.Items[i]) >= addNum)
                    {
                        if ((float)(_contourListBox.Items[i]) == addNum)        //这样做可能会有误差
                            break;                                              //当列表中已经有了这个数的存在就不可以添加了

                        _contourListBox.Items.Insert(i, addNum);
                        break;
                    }
                }
            }
        }

        private void _genButton_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            float min, max, interval;
            if (!float.TryParse(_minTextBox.Text, out min))
            {
                _minTextBox.SelectAll();
                this.ActiveControl = _minTextBox;
                MessageBox.Show("输入数据错误");
                return;
            }
            else
            {
                if (min < _zMin)
                {
                    _minTextBox.SelectAll();
                    this.ActiveControl = _minTextBox;
                    MessageBox.Show("输入数据错误");
                    return;
                }
            }
            if (!float.TryParse(_maxTextBox.Text, out max))
            {
                _maxTextBox.SelectAll();
                this.ActiveControl = _maxTextBox;
                MessageBox.Show("输入数据错误");
                return;
            }
            else
            {
                if (max > _zMax)
                {
                    _maxTextBox.SelectAll();
                    this.ActiveControl = _maxTextBox;
                    MessageBox.Show("输入数据错误（大于最大值）");
                    return;
                }
            }
            if (min > max)
            {
                MessageBox.Show("输入数据错误（最小值大于最大值）");
                return;
            }
            if (!float.TryParse(_intervalTextBox.Text, out interval))
            {
                _intervalTextBox.SelectAll();
                this.ActiveControl = _intervalTextBox;
                MessageBox.Show("输入数据错误");
                return;
            }
            if (interval == 0)
            {
                MessageBox.Show("输入数据错误");
                return;
            }

            _contourListBox.Items.Clear();
            int n = (int)((max - min) / interval + 1);
            for (int i = 0; i < n; i++)
            {
                _contourListBox.Items.Add(min + i * interval);
            }
            _contourListBox.Refresh();
        }
        #endregion 

        public new DialogResult ShowDialog()
        {
            SetListBox();

            base.ShowDialog();

            if (_dialogResult == DialogResult.OK)
            {
                return DialogResult.OK;
            }

            return DialogResult.Cancel;
        }

        private void SetListBox()
        {
            _zMinTextBox.Text = _zMin.ToString();
            _zMaxTextBox.Text = _zMax.ToString();

            for (int i = 0; i < _contourLevel.Length; i++)
            {
                _contourListBox.Items.Add(_contourLevel[i]);
            }

        }

        public float[] ContourLevel
        {
            get
            {
                return _contourLevel;
            }
            set
            {
                
                _contourLevel = value;

                if (_contourLevel.Length == 0)
                    return;

                _zMax = _contourLevel[_contourLevel.Length - 1];
                _zMin = _contourLevel[0];
            }
        }

        public float ZMin
        {
            set
            {
                _zMin = value;
            }
        }

        public float ZMax
        {
            set
            {
                _zMax = value;
            }
        }
    }
}
