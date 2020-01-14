using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ContourTracker03
{
    public class SmoothContour
    {
        /*这个类的作用就是要对曲线进行光滑处理，在这里的光滑处理采用的是抛物线样条
         * 函数来做的，一般的，对曲线的光滑有两种情况，
         * 1）封闭曲线；2）不封闭曲线，这两种情况的处理是不同的，我们就要分别来处理
         */

        //此数控制着插值的点数，默认为10
        public static int Clip = 10;        //Clip必须大于0

        //只要传来所有的未光滑的等值线
        //经过此函数处理以后就得到了所有的光滑过的等值线
        public static List<ExtPoint> SmoothingContour(List<IsoPointListInfo> smoothingList)
        {
            List<ExtPoint> smoothedList = new List<ExtPoint>();

            foreach (IsoPointListInfo aIsoPointListInfo in smoothingList)
            {
                List<IsoPoint> aIsoPointList = aIsoPointListInfo._aIsoPointList;
                aIsoPointList = ListClone(aIsoPointList);

                List<PointF> aSmoothedList;

                if ((aIsoPointList[0]._row == aIsoPointList[aIsoPointList.Count - 1]._row) &&
                (aIsoPointList[0]._column == aIsoPointList[aIsoPointList.Count - 1]._column) &&
                (aIsoPointList[0]._isHorizon == aIsoPointList[aIsoPointList.Count - 1]._isHorizon))
                    aSmoothedList = SmoothingClosedContour(aIsoPointList);
                else
                    aSmoothedList = SmoothingNonClosedContour(aIsoPointList);


                ExtPoint extPoint = new ExtPoint();
                extPoint.contourList = aSmoothedList;
                extPoint.contourValue = aIsoPointListInfo._value;
                smoothedList.Add(extPoint);
            }

            return smoothedList;
        }

        //光滑开曲线
        private static List<PointF> SmoothingNonClosedContour(List<IsoPoint> aIsoPointList)
        {
            List<PointF> aSmoothedList = new List<PointF>();

            float interX = aIsoPointList[1]._x - aIsoPointList[0]._x;
            float interY = aIsoPointList[1]._y - aIsoPointList[0]._y;
            IsoPoint isoPoint = new IsoPoint();
            isoPoint._x = aIsoPointList[0]._x - interX;
            isoPoint._y = aIsoPointList[0]._y - interY;
            aIsoPointList.Insert(0, isoPoint);

            interX = aIsoPointList[aIsoPointList.Count - 1]._x - aIsoPointList[aIsoPointList.Count - 2]._x;
            interY = aIsoPointList[aIsoPointList.Count - 1]._y - aIsoPointList[aIsoPointList.Count - 2]._y;
            isoPoint = new IsoPoint();
            isoPoint._x = interX + aIsoPointList[aIsoPointList.Count - 1]._x;
            isoPoint._y = interY + aIsoPointList[aIsoPointList.Count - 1]._y;
            aIsoPointList.Add(isoPoint);

            double t1, t2, t3, t, a, b, c, d, x, y;
            t = 0.6f / Clip;

            for (int i = 0; i < aIsoPointList.Count - 3; i++)
                for (int j = 1; j < Clip; j++)
                {
                    t1 = j * t;
                    t2 = t1 * t1;
                    t3 = t2 * t1;
                    a = 4.0 * t2 - t1 - 4.0 * t3;
                    b = 1.0 - 10.0 * t2 + 12.0 * t3;
                    c = t1 + 8.0 * t2 - 12.0 * t3;
                    d = 4.0 * t3 - 2.0 * t2;
                    x = a * aIsoPointList[i]._x + b * aIsoPointList[i + 1]._x + c * aIsoPointList[i + 2]._x + d * aIsoPointList[i + 3]._x;
                    y = a * aIsoPointList[i]._y + b * aIsoPointList[i + 1]._y + c * aIsoPointList[i + 2]._y + d * aIsoPointList[i + 3]._y;

                    aSmoothedList.Add(new PointF((float)x, (float)y));
                }
            aSmoothedList.Insert(0, new PointF(aIsoPointList[1]._x, aIsoPointList[1]._y));
            aSmoothedList[aSmoothedList.Count - 1] = new PointF(aIsoPointList[aIsoPointList.Count - 2]._x, aIsoPointList[aIsoPointList.Count - 2]._y);

            return aSmoothedList;
        }

        //光滑封闭曲线
        private static List<PointF> SmoothingClosedContour(List<IsoPoint> aIsoPointList)
        {
            List<PointF> aSmoothedList = new List<PointF>();

            aIsoPointList.Add(new IsoPoint(aIsoPointList[1]));
            aIsoPointList.Insert(0, new IsoPoint(aIsoPointList[aIsoPointList.Count - 3]));

            double t1, t2, t3, t, a, b, c, d, x, y;
            t = 0.6f / Clip;

            for (int i = 0; i < aIsoPointList.Count - 3; i++)
                for (int j = 1; j < Clip; j++)
                {
                    t1 = j * t;
                    t2 = t1 * t1;
                    t3 = t2 * t1;
                    a = 4.0 * t2 - t1 - 4.0 * t3;
                    b = 1.0 - 10.0 * t2 + 12.0 * t3;
                    c = t1 + 8.0 * t2 - 12.0 * t3;
                    d = 4.0 * t3 - 2.0 * t2;
                    x = a * aIsoPointList[i]._x + b * aIsoPointList[i + 1]._x + c * aIsoPointList[i + 2]._x + d * aIsoPointList[i + 3]._x;
                    y = a * aIsoPointList[i]._y + b * aIsoPointList[i + 1]._y + c * aIsoPointList[i + 2]._y + d * aIsoPointList[i + 3]._y;

                    aSmoothedList.Add(new PointF((float)x, (float)y));
                }
            aSmoothedList.Insert(0, new PointF(aIsoPointList[1]._x, aIsoPointList[1]._y));
            aSmoothedList[aSmoothedList.Count - 1] = new PointF(aIsoPointList[aIsoPointList.Count - 2]._x, aIsoPointList[aIsoPointList.Count - 2]._y);

            return aSmoothedList;
        }

        private static List<IsoPoint> ListClone(List<IsoPoint> list)
        {
            IsoPoint[] isoPointArray = new IsoPoint[list.Count];
            list.CopyTo(isoPointArray);

            return new List<IsoPoint>(isoPointArray);
        }
    }

    public struct ExtPoint
    {
        public List<PointF> contourList;
        
        public float contourValue;

    }
}
