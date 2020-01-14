﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContourTracker02
{
    public class BusinessContour
    {
        public const float Epsilon = 0.000001f;
        //说明：
        //在程序中有一个用于浮点数的比较问题，其中就有一个＝＝的情况，在这种情况下是不好用＝＝的
        //有的程序中用到＝＝这样的一种比较方式，虽然在那个程序中没有问题，但是我觉得那是不好的，在这里，我
        //认为两者值之间的绝对值之差小于Epsilon时就认为相等

        public const int NOfContourLevel = 10;
        private const float Excursion = 0.001f;     //修正值
        //这个修正值是由于奇点（奇点就是位于网格十字叉叉心上的等值点），这个叉点的存在会使在追踪时
        //可能被追踪四次，这样会形成等值线交叉，是不对的，为了避免这种情况，可以人为的在计算时，给
        //网格点加上一个微小的值（修正值），这样得到的等值点就不会出现在叉点上了，而又不会对最终的
        //结果有大的影响

        private AccessContour _accessContour;
        //private bool _isSetDefaultContourLevel = true;  //是否设置成为默认的等值线等级，默认为10等分
        //                                                //等值线表－－就是要求的等值线的集合

        private float[] _contourLevel;      //要求的等值线

        //每条边上的信息（有无等值点，有时点在何处（_rate）
        struct EdgeIsoInfo         
        {
            public float _rate;     //比率是代表在网格边上的比率位置
            public bool _isIsoPoint;   //在此边上是否有等值点
        }

        private EdgeIsoInfo[,] _xSide;
        private EdgeIsoInfo[,] _ySide;

        private float _deltX;
        private float _deltY;

        private string _preFileName = null;     //先前的文件名，来决定是否再次读取数据

        private float _curFollowingValue;       //当前正在追踪的等值线值

        private IsoPoint _preIsoPoint;
        private IsoPoint _curIsoPoint;
        private IsoPoint _nextIsoPoint;

        private IsoPointListInfo _isoPointListInfo;
        private List<IsoPointListInfo> _allIsoPointList;                //所有等值线链表

        public event FollowingFinish FollowingFinishHandle;

        public BusinessContour()
        {
            _accessContour = new AccessContour();
        }

        //一系列得到等值线操作的启动器
        public void BeginSeriesAction()
        {
            _allIsoPointList = new List<IsoPointListInfo>();

            if (_preFileName != _accessContour.FileName)
            {
                _accessContour.GetGridData();
                _preFileName = _accessContour.FileName;
                
                SetDefaultContourLevel();           //否则就由用户自己修改
            }

            if (_accessContour.FileName == null)
                return;

            if (ContourLevel.Length == 0)
                return;

            //if (_isSetDefaultContourLevel)
            //    SetDefaultContourLevel();           //否则就由用户自己修改

            //每一小格的X间距，Y间距，对于一个网格是一定的，所以就只要求一次
            //放在这里仅仅是因为为了提高一点效率
            _deltX = (_accessContour.GridInfo._xMax - _accessContour.GridInfo._xMin) / (_accessContour.GridInfo._columns - 1);
            _deltY = (_accessContour.GridInfo._yMax - _accessContour.GridInfo._yMin) / (_accessContour.GridInfo._rows - 1);

            AllocateMemory();       //分配边信息内存

            for (int i = 0; i < _contourLevel.Length; i++)
            {
                _curFollowingValue = _contourLevel[i];

                InterpolateTracingValue(); //扫描并计算纵、横边上等值点的情况

                TracingNonClosedContour();  //追踪开等值线

                TrackingClosedContour();    //追踪封闭等值线
            }

            //FollowingFinishHandle();                //这里的这种事件通知的方式究竟好不好呢？
        }

        private void AllocateMemory()
        {
            _xSide = new EdgeIsoInfo[_accessContour.GridInfo._rows, _accessContour.GridInfo._columns - 1];
            _ySide = new EdgeIsoInfo[_accessContour.GridInfo._rows - 1, _accessContour.GridInfo._columns];
        }

        //扫描并计算横、边纵上等值点的情况（得到_xSide, _ySide的值）
        private void InterpolateTracingValue()
        {
            /*      网格点标识如下:
        
            (i+1,j)·--------·(i+1,j+1)
                    |        |
                    |        |
                    ·       |
	                |        |
	         (i,j) ·--------·(i,j+1)

              i:表示行号(向上增加)
			  j:表示列号(向右增加)
			  标识一个网格交点时，行号在前，列号在右，如：(i,j)
            */

            /*
             * 其中对水平方向h0--(i, j)   h1--(i, j+1)  _xSide, _ySide中的_rate为(value-h0)/(h1-h0)
             */
            if (_xSide == null || _ySide == null)
                throw new Exception("分配内存错误！");

            int i = 0;
            int j = 0;
            float h0, h1;       //计录一条边上的两个值
            float flag;        //中间值

            GridPoint[] gridPoints = _accessContour.GridPoints;

            int rows = _accessContour.GridInfo._rows;               //网格的行列数
            int columns = _accessContour.GridInfo._columns;         //这里增加这两个变量，仅仅是为了减少程序的中转
                                                                    //增加效率

            int edgeRows= _accessContour.GridInfo._rows;                //这里的行列数是根据横，纵边来定的
            int edgeColumns = _accessContour.GridInfo._columns - 1;     //这里先计算的是横向的

            for (i = 0; i < edgeRows; i++)
                for (j = 0; j < edgeColumns; j++)
                {
                    h0 = gridPoints[i * columns + j]._z;
                    h1 = gridPoints[i * columns + j + 1]._z;

                    if (Math.Abs(h0 - h1) < Epsilon)
                    {
                        _xSide[i, j]._rate = -2.0f;
                        _xSide[i, j]._isIsoPoint = false;
                    }
                    else
                    {
                        flag = (_curFollowingValue - h0) * (_curFollowingValue - h1);

                        if (flag > 0)
                        {
                            _xSide[i, j]._rate = -2.0f;
                            _xSide[i, j]._isIsoPoint = false;
                        }
                        else if (flag < 0)
                        {
                            _xSide[i, j]._rate = (_curFollowingValue - h0) / (h1 - h0);
                            _xSide[i, j]._isIsoPoint = true;
                        }
                        else
                        {
                            if ((_curFollowingValue - h0) < Epsilon && (_curFollowingValue - h0) > -Epsilon)
                                h0 += Excursion;
                            else
                                h1 += Excursion;

                            _xSide[i, j]._rate = (_curFollowingValue - h0) / (h1 - h0);

                            if (_xSide[i, j]._rate < 0 || _xSide[i, j]._rate > 1)
                                _xSide[i, j]._isIsoPoint = false;
                            else 
                                _xSide[i, j]._isIsoPoint = true;
                        }
                    }
                }

            edgeRows = _accessContour.GridInfo._rows - 1;
            edgeColumns = _accessContour.GridInfo._columns;               //再计算纵向的

            for (i=0; i<edgeRows; i++)
                for (j = 0; j < edgeColumns; j++)
                {
                    h0 = gridPoints[i * columns + j]._z;
                    h1 = gridPoints[(i + 1) * columns + j]._z;

                    if (Math.Abs(h0 - h1) < Epsilon)
                    {
                        _ySide[i, j]._rate = -2.0f;
                        _ySide[i, j]._isIsoPoint = false;
                    }
                    else
                    {
                        flag = (_curFollowingValue - h0) * (_curFollowingValue - h1);

                        if (flag > 0)
                        {
                            _ySide[i, j]._rate = -2.0f;
                            _ySide[i, j]._isIsoPoint = false;
                        }
                        else if (flag < 0)
                        {
                            _ySide[i, j]._rate = (_curFollowingValue - h0) / (h1 - h0);
                            _ySide[i, j]._isIsoPoint = true;
                        }
                        else
                        {
                            if ((_curFollowingValue - h0) < Epsilon && (_curFollowingValue - h0) > -Epsilon)
                                h0 += Excursion;
                            else
                                h1 += Excursion;

                            _ySide[i, j]._rate = (_curFollowingValue - h0) / (h1 - h0);

                            if (_ySide[i, j]._rate < 0 || _ySide[i, j]._rate > 1)
                                _ySide[i, j]._isIsoPoint = false;
                            else
                                _ySide[i, j]._isIsoPoint = true;
                        }
                    }
                }
        }

        //追踪开等值线
        private void TracingNonClosedContour()
        {
            /*
             * 这里的内存布局可以认为是     ^   |-------------|
             * 在竖直方向向上递增           |   |      |      |
             * 在水平方向向右递增           |   |-------------|
             *                              |   |      |      |
             *                              |   |-------------|
             *                              |-------------------->
             *由于在追踪时的方向不同，就要分成不同的情况
             */
            int edgeRows = _accessContour.GridInfo._rows - 1;
            int edgeColumns = _accessContour.GridInfo._columns - 1;
            
            //约定俗成i表示行，j表示列
            //追踪底边框
            for (int j = 0; j < edgeColumns; j++)       
            {
                if (_xSide[0, j]._isIsoPoint)
                {
                    _preIsoPoint._row = -1;
                    _preIsoPoint._column = j;
                    _preIsoPoint._isHorizon = true;
                    _curIsoPoint._row = 0;
                    _curIsoPoint._column = j;
                    _curIsoPoint._isHorizon = true;

                    TracingOneNonClosedContour();
                }
            }

            //追踪左边框
            for (int i = 0; i < edgeRows; i++)
            {
                if (_ySide[i, 0]._isIsoPoint)
                {
                    _preIsoPoint._row = i;
                    _preIsoPoint._column = -1;
                    _preIsoPoint._isHorizon = false;
                    _curIsoPoint._row = i;
                    _curIsoPoint._column = 0;
                    _curIsoPoint._isHorizon = false;

                    TracingOneNonClosedContour();
                }
            }

            //追踪上边框
            for (int j = 0; j < edgeColumns; j++)
            {
                if (_xSide[edgeRows, j]._isIsoPoint)
                {
                    _preIsoPoint._row = edgeRows;
                    _preIsoPoint._column = j;
                    _preIsoPoint._isHorizon = true;
                    _curIsoPoint._row = edgeRows;
                    _curIsoPoint._column = j;
                    _curIsoPoint._isHorizon = true;

                    TracingOneNonClosedContour();
                }
            }

            //追踪右边框
            for (int i = 0; i < edgeRows; i++)
            {
                if (_ySide[i, edgeColumns]._isIsoPoint)
                {
                    _preIsoPoint._row = i;
                    _preIsoPoint._column = edgeColumns;
                    _preIsoPoint._isHorizon = false;
                    _curIsoPoint._row = i;
                    _curIsoPoint._column = edgeColumns;
                    _curIsoPoint._isHorizon = false;

                    TracingOneNonClosedContour();
                }
            }
        }

        //追踪封闭等值线
        private void TrackingClosedContour()
        {
            int rows = _accessContour.GridInfo._rows - 1;
            int columns = _accessContour.GridInfo._columns - 1;

            int i = 0;
            int j = 0;
            for(j = 1 ; j < columns; j++)
                for (i = 0; i < rows; i++)
                {
                    if (_ySide[i, j]._isIsoPoint)
                    {
                        _preIsoPoint._row = i;
                        _preIsoPoint._column = 0;
                        _preIsoPoint._isHorizon = false;
                        _curIsoPoint._row = i;
                        _curIsoPoint._column = j;
                        _curIsoPoint._isHorizon = false;

                        TracingOneClosedContour();
                    }
                }
        }

        //追踪一条封闭的等值线
        private void TracingOneClosedContour()
        {
            int rows = _accessContour.GridInfo._rows;       //这里是网格点的行列数
            int columns = _accessContour.GridInfo._columns;

            int startI = _curIsoPoint._row;
            int startJ = _curIsoPoint._column;

            _isoPointListInfo = new IsoPointListInfo();
            List<IsoPoint> isoPointList = new List<IsoPoint>();
            _isoPointListInfo._aIsoPointList = isoPointList;
            _isoPointListInfo._value = _curFollowingValue;

            CalcCoord(_curIsoPoint._row, _curIsoPoint._column, false);

            TracingNextPoint();

            _preIsoPoint = _curIsoPoint;
            _curIsoPoint = _nextIsoPoint;

            bool isClosed = false;

            while (!isClosed)
            {
                TracingNextPoint();

                _preIsoPoint = _curIsoPoint;
                _curIsoPoint = _nextIsoPoint;

                isClosed = (_curIsoPoint._row == startI) && (_curIsoPoint._column == startJ) && (false == _curIsoPoint._isHorizon);
            }

            _allIsoPointList.Add(_isoPointListInfo);
        }

        //追踪一条等值线
        //这个函数要完成一条等值线追踪的所有操作
        private void TracingOneNonClosedContour()
        {
            int rows = _accessContour.GridInfo._rows;       //这里是网格点的行列数
            int columns = _accessContour.GridInfo._columns;

            _isoPointListInfo = new IsoPointListInfo();
            List<IsoPoint> isoPointList = new List<IsoPoint>();
            _isoPointListInfo._aIsoPointList = isoPointList;
            _isoPointListInfo._value = _curFollowingValue;

            CalcCoord(_curIsoPoint._row, _curIsoPoint._column, _curIsoPoint._isHorizon);

            if (_curIsoPoint._isHorizon)
                _xSide[_curIsoPoint._row, _curIsoPoint._column]._isIsoPoint = false;
            else
                _ySide[_curIsoPoint._row, _curIsoPoint._column]._isIsoPoint = false;
            
            TracingNextPoint();

            _preIsoPoint = _curIsoPoint;
            _curIsoPoint = _nextIsoPoint;

            bool isFinish = (_curIsoPoint._row == 0 && _curIsoPoint._isHorizon) || (_curIsoPoint._column == 0 && !_curIsoPoint._isHorizon)
                || (_curIsoPoint._row == rows - 1) || (_curIsoPoint._column == columns - 1);

            while (!isFinish)
            {
                TracingNextPoint();

                _preIsoPoint = _curIsoPoint;
                _curIsoPoint = _nextIsoPoint;

                isFinish = (_curIsoPoint._row == 0 && _curIsoPoint._isHorizon) || (_curIsoPoint._column == 0 && !_curIsoPoint._isHorizon)
                || (_curIsoPoint._row == rows - 1) || (_curIsoPoint._column == columns - 1);
            }

            _allIsoPointList.Add(_isoPointListInfo);
        }

        private void TracingNextPoint()
        {
            if (_curIsoPoint._row > _preIsoPoint._row)
            {
                TracingFromBottom2Top();
                return;
            }
            else if (_curIsoPoint._column > _preIsoPoint._column)
            {
                TracingFromLeft2Right();
                return;
            }
            else if (_curIsoPoint._isHorizon)
            {
                TracingFromTop2Bottom();
                return;
            }
            else
            {
                TracingFromRight2Left();
                return;
            }
        }

        //自下向上追踪函数
        private void TracingFromBottom2Top()
        {
            int row = _curIsoPoint._row;
            int column = _curIsoPoint._column;

            if (_ySide[row, column]._isIsoPoint)
            {
                if (_xSide[row + 1, column]._isIsoPoint)
                {
                    if (_ySide[row, column]._rate < _ySide[row, column + 1]._rate)
                    {
                        HandlingAfterNextPointFound(row, column, false);
                        return;
                    }
                    else if (_ySide[row, column]._rate > _ySide[row, column + 1]._rate)
                    {
                        HandlingAfterNextPointFound(row, column + 1, false);
                        return;
                    }
                    else
                    {
                        if (_ySide[row + 1, column]._rate < 0.5f)
                        {
                            HandlingAfterNextPointFound(row, column, false);
                            return;
                        }
                        else
                        {
                            HandlingAfterNextPointFound(row, column + 1, false);
                            return;
                        }
                    }
                }
                else
                {
                    HandlingAfterNextPointFound(row, column, false);
                    return;
                }
            }
            else if (_xSide[row + 1, column]._isIsoPoint)
            {
                HandlingAfterNextPointFound(row + 1, column, true);
                return;
            }
            else
                HandlingAfterNextPointFound(row, column + 1, false);
        }

        //自左向右追踪函数
        private void TracingFromLeft2Right()
        {
            int row = _curIsoPoint._row;
            int column = _curIsoPoint._column;

            if (_xSide[row + 1, column]._isIsoPoint)
            {
                if (_ySide[row, column + 1]._isIsoPoint)
                {
                    if (_ySide[row + 1, column]._rate < _ySide[row, column]._rate)
                    {
                        HandlingAfterNextPointFound(row + 1, column, false);
                        return;
                    }
                    else if (_ySide[row + 1, column]._rate > _ySide[row, column]._rate)
                    {
                        HandlingAfterNextPointFound(row, column, false);
                        return;
                    }
                    else
                    {
                        if (_ySide[row, column + 1]._rate < 0.5f)
                        {
                            HandlingAfterNextPointFound(row + 1, column, true);
                            return;
                        }
                        else
                        {
                            HandlingAfterNextPointFound(row, column, true);
                            return;
                        }
                    }
                }
                else
                {
                    HandlingAfterNextPointFound(row + 1, column, true);
                    return;
                }
            }
            else if (_ySide[row, column + 1]._isIsoPoint)
            {
                HandlingAfterNextPointFound(row, column + 1, false);
                return;
            }
            else
            {
                HandlingAfterNextPointFound(row, column, true);
                return;
            }
        }
        
        //自上向下追踪函数
        private void TracingFromTop2Bottom()
        {
            int row = _curIsoPoint._row;
            int column = _curIsoPoint._column;

            if (_ySide[row - 1, column + 1]._isIsoPoint)
            {
                if (_xSide[row - 1, column]._isIsoPoint)
                {
                    if (_ySide[row - 1, column]._rate < _ySide[row - 1, column + 1]._rate)      //这里要注意与由上到下的不同的处理
                    {
                        HandlingAfterNextPointFound(row - 1, column + 1, false);
                        return;
                    }
                    else if (_ySide[row - 1, column]._rate > _ySide[row - 1, column + 1]._rate)
                    {
                        HandlingAfterNextPointFound(row - 1, column, false);
                        return;
                    }
                    else
                    {
                        if (_xSide[row - 1, column]._rate < 0.5f)
                        {
                            HandlingAfterNextPointFound(row - 1, column, false);
                            return;
                        }
                        else
                        {
                            HandlingAfterNextPointFound(row - 1, column + 1, false);
                            return;
                        }
                    }

                }
                else
                {
                    HandlingAfterNextPointFound(row - 1, column + 1, false);
                    return;
                }
            }
            else if (_xSide[row - 1, column]._isIsoPoint)
            {
                HandlingAfterNextPointFound(row - 1, column, true);
                return;
            }
            else
            {
                HandlingAfterNextPointFound(row - 1, column, false);
                return;
            }
        }

        //自右向左追踪函数
        private void TracingFromRight2Left()
        {
            int row = _curIsoPoint._row;
            int column = _curIsoPoint._column;

            if (_xSide[row, column - 1]._isIsoPoint)
            {
                if (_ySide[row, column - 1]._isIsoPoint)
                {
                    if (_xSide[row, column - 1]._rate < _xSide[row + 1, column - 1]._rate)
                    {
                        HandlingAfterNextPointFound(row + 1, column - 1, true);
                        return;
                    }
                    else if (_xSide[row, column - 1]._rate > _xSide[row + 1, column - 1]._rate)
                    {
                        HandlingAfterNextPointFound(row, column - 1, true);
                        return;
                    }
                    else
                    {
                        if (_ySide[row, column - 1]._rate < 0.5f)
                        {
                            HandlingAfterNextPointFound(row, column - 1, true);
                            return;
                        }
                        else
                        {
                            HandlingAfterNextPointFound(row + 1, column - 1, true);
                            return;
                        }
                    }
                }
                else
                {
                    HandlingAfterNextPointFound(row, column - 1, true);
                    return;
                }
            }
            else if (_ySide[row, column - 1]._isIsoPoint)
            {
                HandlingAfterNextPointFound(row, column - 1, false);
                return;
            }
            else
            {
                HandlingAfterNextPointFound(row + 1, column - 1, true);
                return;
            }
        }

        //下一个点找到后的处理
        //得到下一个点的行列数，并计算当前点的坐标，并且这里有一个要注意的
        //地方，当前等值点计算过后在将标记为没有等值点，不然的话就会同一点设置多次
        //而得到错误结果
        private void HandlingAfterNextPointFound(int row, int column, bool isHorizon)
        {
            if (row >= 0 && (row <= _accessContour.GridInfo._rows - 1) && column >= 0 && column <= (_accessContour.GridInfo._columns - 1))
            {
                _nextIsoPoint._row = row;
                _nextIsoPoint._column = column;
                _nextIsoPoint._isHorizon = isHorizon;

                CalcCoord(row, column, isHorizon);

                if (isHorizon)
                    _xSide[row, column]._isIsoPoint = false;
                else
                    _ySide[row, column]._isIsoPoint = false;
            }
            else
            {
                throw new Exception("追踪过程出现错误");
            }
        }

        //计算当前等值点所在的坐标
        private void CalcCoord(int row, int column, bool isHorizon)
        {
            IsoPoint isoPoint = new IsoPoint();
            isoPoint._column = column;
            isoPoint._row = row;
            isoPoint._isHorizon = isHorizon;

            if (isHorizon)
            {
                isoPoint._x = _accessContour.GridInfo._xMin + (column + _xSide[row, column]._rate) * _deltX;
                isoPoint._y = _accessContour.GridInfo._yMin + row * _deltY;
            }
            else
            {
                isoPoint._x = _accessContour.GridInfo._xMin + column * _deltX;
                isoPoint._y = _accessContour.GridInfo._yMin + (row + _ySide[row, column]._rate) * _deltY;
            }

            float value = _curFollowingValue;
            _isoPointListInfo._aIsoPointList.Add(isoPoint);
        }

        //用户修改等值线等级表
        private void SetContourLevel(float [] contourLevel)
        {
            //用户自己修改就应该由用户自己传来要求的等值线
            _contourLevel = contourLevel;   
        }

        //默认的等值线（10等分）
        private void SetDefaultContourLevel()
        {
            _contourLevel = new float[NOfContourLevel];

            for (int i = 0; i < NOfContourLevel; i++)
            {
                _contourLevel[i] = (_accessContour.GridInfo._zMax * i + (NOfContourLevel - i - 1) * _accessContour.GridInfo._zMin)
                                                    / (NOfContourLevel - 1);
            }
        }

        //public void SetFileName(string fileName)
        //{
        //    _preString = _accessContour.FileName;
        //    _accessContour.FileName = fileName;
        //}

        public string FileName
        {
            get
            {
                return _accessContour.FileName;
            }
            set
            {
                _accessContour.FileName = value;
            }
        }

        public List<IsoPointListInfo> AllIsoPointList
        {
            get
            {
                return _allIsoPointList;
            }
        }

        //public bool IsDefaultContourLevel
        //{
        //    set
        //    {
        //        _isSetDefaultContourLevel = value;
        //    }
        //}

        public float[] ContourLevel
        {
            get
            {
                return _contourLevel;
            }
            set
            {
                _contourLevel = value;
            }
        }

        public GridInfo GridInfo
        {
            get
            {
                return _accessContour.GridInfo;
            }
        }
    }

    public delegate void FollowingFinish();

    public struct IsoPoint
    {
        public int _column;         //这里的行列的最大值要比网格的行列数分别小1
        public int _row;            //因为这里的行列是按行列线（注意是线）来算的

        public float _x;        //当前等值点所在的坐标位置
        public float _y;

        public bool _isHorizon;     //等值点是否在X轴上（水平线上）   true--X   false--Y

        public IsoPoint(IsoPoint isoPoint)
        {
            _column = isoPoint._column;
            _row = isoPoint._row;

            _x = isoPoint._x;
            _y = isoPoint._y;

            _isHorizon = isoPoint._isHorizon;
        }
    }

    public struct IsoPointListInfo     //等值线的信息
    {
        public List<IsoPoint> _aIsoPointList;
        public float _value;     //指示当前等值线的值
    }
}
