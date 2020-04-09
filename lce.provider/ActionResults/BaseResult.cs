/* file name：${namespace}.BaseResult.cs
* author：lynx <lynx.kor@163.com> @ 2020/3/14 11:45
* copyright (c) 2020 Copyright@lynxce.com
* desc：
* > add description for BaseResult
* revision：
*
*/

using lce.provider.Enums;

namespace lce.provider.ActionResults
{
    /// <summary>
    /// 请求响应体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseResult<T>
    {
        /// <summary>
        /// 请求响应体
        /// </summary>
        public BaseResult() { }

        /// <summary>
        /// 请求响应体
        /// </summary>
        /// <param name="code">状态码</param>
        public BaseResult(ResponseCode code) : this()
        {
            Code = code;
            Msg = code.Description();
        }

        /// <summary>
        /// 请求响应体
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="msg">状态信息</param>
        public BaseResult(ResponseCode code, string msg) : this()
        {
            Code = code;
            Msg = msg;
        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="data">数据体</param>
        public BaseResult(T data) : this()
        {
            Data = data;
        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="data">数据体</param>
        public BaseResult(ResponseCode code, T data) : this()
        {
            Code = code;
            Msg = code.Description();
            Data = data;
        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="msg">状态信息</param>
        /// <param name="data">数据体</param>
        public BaseResult(ResponseCode code, string msg, T data) : this()
        {
            Code = code;
            Msg = msg;
            Data = data;
        }

        /// <summary>
        /// 状态码
        /// </summary>
        public ResponseCode Code { get; set; } = ResponseCode.SUCCESS;

        /// <summary>
        /// 状态信息
        /// </summary>
        public string Msg { get; set; } = ResponseCode.SUCCESS.Description();

        /// <summary>
        /// 返回的数据内容
        /// </summary>
        public T Data { get; set; }
    }
}
