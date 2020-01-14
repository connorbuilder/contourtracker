using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ContourTracker03
{
    public class AccessContour
    {
        public const float Epsilon = 0.000001f;

        protected GridInfo _gridInfo;
        protected GridPoint[] _gridPoints;    //一维数组存储所有的网格点

        public AccessContour()
        {
            _gridInfo._rows = 0;
            _gridInfo._columns = 0;

            _gridInfo._xMin = float.MaxValue;       //为以后得到最大最小值做准备
            _gridInfo._xMax = float.MinValue;
            _gridInfo._yMin = float.MaxValue;
            _gridInfo._yMax = float.MinValue;
            _gridInfo._zMin = float.MaxValue;
            _gridInfo._zMax = float.MinValue;
        }

        public virtual void GetGridData()
        {
        }

        public GridInfo GridInfo
        {
            get
            {
                return _gridInfo;       //这个值外界是无权修改的，只能读
            }
        }

        public GridPoint[] GridPoints
        {
            get
            {
                return _gridPoints;
            }
        }

        public virtual string FileName
        {
            get;
            set;
        }
    }


    //整张网格数据的一些基本信息
    //包括数据风格的行数，列数，X，Y,Z上的最大，最小值
    public struct GridInfo
    {
        public int _rows;
        public int _columns;

        public float _xMin;
        public float _xMax;
        public float _yMin;
        public float _yMax;
        public float _zMin;
        public float _zMax;
    }

    //网格点上值
    public struct GridPoint
    {
        public float _x;
        public float _y;
        public float _z;
    }
}
