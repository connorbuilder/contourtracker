using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ContourTracker03
{
    class AccessContourFile : AccessContour
    {
        //当前选中的文件
        protected string _fileName;
        
        public AccessContourFile()
            : base()
        {
        }

        //这里有两种情况
        //由于现在有两种数据格式（根据现在存在的数据文件而言），一种是行优先(X轴)存放的，另一种是列优先(Y轴)存放的
        //为了不在两种文件处理的过程中出现问题，所以做了分别的处理
        //使得到的数据都是行优先
        //比如由Surface得到的就是行优先的，而我们得到的层位数据一般是列优先的
        public override void GetGridData()
        {
            if (_fileName == null)
                throw new Exception("请选择文件");

            try
            {
                StreamReader fileStream = new StreamReader(_fileName, Encoding.Default);
                string textContent = fileStream.ReadToEnd();
                fileStream.Close();

                string[] textLines = textContent.Trim().Replace("\r\n", "\n").Replace("\t", " ").Split('\n');

                string[] lineContent0 = textLines[0].Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string[] lineContent1 = textLines[1].Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (Math.Abs(Convert.ToSingle(lineContent0[0]) - Convert.ToSingle(lineContent1[0])) < Epsilon)
                {
                    GetGridDataFirstColumn(textLines);     //原始文件数据为列优先
                }
                else
                {
                    GetGridDataFirstRow(textLines);  //原始文件数据为行优先（Surface文件类型）
                }
            }
            catch
            {
                throw new Exception("文件无法识别");
            }
        }

        //原始文件数据为行优先（Surface文件类型）
        private void GetGridDataFirstRow(string[] textLines)
        {
            _gridPoints = new GridPoint[textLines.Length];

            //原文件为行优先存储时的这里的处理简单一点
            string[] lineContent;
            for (int i = 0; i < textLines.Length; i++)
            {
                lineContent = textLines[i].Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                _gridPoints[i]._x = Convert.ToSingle(lineContent[0]);
                _gridPoints[i]._y = Convert.ToSingle(lineContent[1]);
                _gridPoints[i]._z = Convert.ToSingle(lineContent[2]);

                if (_gridPoints[i]._x > _gridInfo._xMax)         //分别求得网格的X,Y,Z的最大最小值
                    _gridInfo._xMax = _gridPoints[i]._x;
                if (_gridPoints[i]._x < _gridInfo._xMin)
                    _gridInfo._xMin = _gridPoints[i]._x;

                if (_gridPoints[i]._y > _gridInfo._yMax)
                    _gridInfo._yMax = _gridPoints[i]._y;
                if (_gridPoints[i]._y < _gridInfo._yMin)
                    _gridInfo._yMin = _gridPoints[i]._y;

                if (_gridPoints[i]._z > _gridInfo._zMax)
                    _gridInfo._zMax = _gridPoints[i]._z;
                if (_gridPoints[i]._z < _gridInfo._zMin)
                    _gridInfo._zMin = _gridPoints[i]._z;
            }

            //网格数据的X, Y轴方向间隔是相等的
            //行列数就可以通过列的最大最小值得到
            _gridInfo._columns = (int)((_gridInfo._xMax - _gridInfo._xMin) / (_gridPoints[1]._x - _gridPoints[0]._x) + 1) + 1;
            _gridInfo._rows = (int)(_gridPoints.Length / _gridInfo._columns);
        }

        //原始文件数据为列优先
        private void GetGridDataFirstColumn(string[] textLines)
        {
            _gridPoints = new GridPoint[textLines.Length];

            //{先得到列数 再由列数得到行数（这一步是由于按行存储而来的)
            string[] lineContent = textLines[0].Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            float temp = Convert.ToSingle(lineContent[0]);
            float next;
            for (int i = 0; i < textLines.Length; i++)
            {
                lineContent = textLines[i].Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                next = Convert.ToSingle(lineContent[0]);
                if (Math.Abs(temp - next) < Epsilon)
                {
                    temp = next;
                }
                else
                {
                    _gridInfo._rows = i;
                    break;
                }
            }
            _gridInfo._columns = textLines.Length / _gridInfo._rows;
            //}

            //现在开始得到网格数据（注意原文件是列优先存储的，我们这里要得到行优先的数据
            //这样好以后处理数据时统一）
            int n = 0;
            int m = 0;
            for (int i = 0; i < _gridInfo._columns; i++)
                for (int j = 0; j < _gridInfo._rows; j++)
                {
                    lineContent = textLines[n++].Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    m = j * _gridInfo._columns + i;
                    _gridPoints[m]._x = Convert.ToSingle(lineContent[0]);
                    _gridPoints[m]._y = Convert.ToSingle(lineContent[1]);
                    _gridPoints[m]._z = Convert.ToSingle(lineContent[2]);

                    if (_gridPoints[m]._x > _gridInfo._xMax)         //分别求得网格的X,Y,Z的最大最小值
                        _gridInfo._xMax = _gridPoints[m]._x;
                    if (_gridPoints[m]._x < _gridInfo._xMin)
                        _gridInfo._xMin = _gridPoints[m]._x;

                    if (_gridPoints[m]._y > _gridInfo._yMax)
                        _gridInfo._yMax = _gridPoints[m]._y;
                    if (_gridPoints[m]._y < _gridInfo._yMin)
                        _gridInfo._yMin = _gridPoints[m]._y;

                    if (_gridPoints[m]._z > _gridInfo._zMax)
                        _gridInfo._zMax = _gridPoints[m]._z;
                    if (_gridPoints[m]._z < _gridInfo._zMin)
                        _gridInfo._zMin = _gridPoints[m]._z;
                }
        }

        public override string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
            }
        }
    }
}
