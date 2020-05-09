﻿/* file name：${namespace}.ResponseCode.cs
* author：lynx <lynx.kor@163.com> @ 2020/3/14 11:20
* copyright (c) 2020 Copyright@lynxce.com
* desc：
* > add description for ResponseCode
* revision：
*
*/

using System.ComponentModel;

namespace lce.provider.Enums
{
    /// <summary>
    /// 响应代码/状态信息
    /// </summary>
    public enum ResponseCode
    {
        /// <summary>
        /// 操作成功
        /// </summary>
        [Description("操作成功")]
        SUCCESS = 200,

        /// <summary>
        /// 请求出错
        /// </summary>
        [Description("请求出错")]
        BAD_REQUEST = 400,

        /// <summary>
        /// 身份验证失败/无权访问
        /// </summary>
        [Description("身份验证失败/无权访问")]
        UN_AUTHORIZED = 401,

        /// <summary>
        /// 请求响应超时
        /// </summary>
        [Description("请求响应超时")]
        REQUEST_TIMEOUT = 408,

        /// <summary>
        /// 服务一般性错误
        /// </summary>
        [Description("服务一般性错误")]
        SERVER_ERROR = 500,
    }
}