/* file name：lce.provider.CoordinateExt.cs
* author：lynx lynx.kor@163.com @ 2021/3/8 10:30:16
* copyright (c) 2021 Copyright@lynxce.com
* desc：
* > add description for CoordinateExt
* revision：
*
*/

using System;

namespace lce.provider
{
    /// <summary>
    /// 经纬度计算器
    /// </summary>
    public static class CoordinateExt
    {
        /// <summary>
        /// 方圆几公里计算
        /// </summary>
        /// <param name="lng">   经度</param>
        /// <param name="lat">   纬度</param>
        /// <param name="minLng"></param>
        /// <param name="maxLng"></param>
        /// <param name="maxLat"></param>
        /// <param name="minLat"></param>
        /// <param name="dis">   距离，默认0.5km</param>
        public static void MilesAround(double lng, double lat
            , out double minLng, out double maxLng
            , out double minLat, out double maxLat
            , double dis = 0.5)
        {
            double r = 6371;//地球半径千米  
            //double dis = 0.5;//0.5千米距离  
            double dlng = 2 * Math.Asin(Math.Sin(dis / (2 * r)) / Math.Cos(lat * Math.PI / 180));
            dlng = dlng * 180 / Math.PI;//角度转为弧度  
            double dlat = dis / r;
            dlat = dlat * 180 / Math.PI;

            minLng = lng - dlng;
            maxLng = lng + dlng;
            maxLat = lat + dlat;
            minLat = lat - dlat;
        }
    }
}