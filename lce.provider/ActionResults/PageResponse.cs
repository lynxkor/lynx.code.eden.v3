/* file name：${namespace}.PageResponse.cs
* author：lynx <lynx.kor@163.com> @ 2020/3/14 11:47
* copyright (c) 2020 Copyright@lynxce.com
* desc：
* > add description for PageResponse
* revision：
*
*/

namespace lce.provider.ActionResults
{
    /// <summary>
    /// 分页/列表请求响应体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageResponse<T> : BaseResponse<T>
    {
        /// <summary>
        /// 实例化
        /// </summary>
        public PageResponse() : base() { }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="size">面阀</param>
        /// <param name="total">总数</param>
        public PageResponse(int page, int size, int total) : base()
        {
            Page = page;
            Size = size;
            Total = total;
        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="size">页阀</param>
        /// <param name="total">总数</param>
        /// <param name="data">数据体</param>
        public PageResponse(int page, int size, int total, T data) : base(data)
        {
            Page = page;
            Size = size;
            Total = total;
        }

        /// <summary>
        /// 页码
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// 页阀
        /// </summary>
        public int Size { get; set; } = 10;

        /// <summary>
        /// 总数
        /// </summary>
        public int Total { get; set; } = 0;
    }
}
