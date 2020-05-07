/* file name：${namespace}.HttpClientExt.cs
* author：lynx <lynx.kor@163.com> @ 2020/4/11 0:45
* copyright (c) 2020 Copyright@lynxce.com
* desc：
* > add description for HttpClientExt
* revision：
*
*/

using lce.provider.Enums;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace lce.provider
{
    /// <summary>
    /// HttpClientExt
    /// </summary>
    public static class HttpClientExt
    {
        /// <summary>
        /// Post
        /// </summary>
        /// <param name="url">        </param>
        /// <param name="param">      </param>
        /// <param name="headers">    </param>
        /// <param name="contentType"></param>
        /// <param name="charSet">    </param>
        /// <returns></returns>
        public static Dictionary<ResponseCode, string> Post(string url
            , Dictionary<string, string> param = null
            , Dictionary<string, string> headers = null
            , string contentType = "application/json", string charSet = "utf-8")
        {
            try
            {
                using var httpClient = new HttpClient();
                var body = "";
                if (null != param && param.Count > 0)
                {
                    body = param.ToJson();
                }
                HttpResponseMessage response = null;
                using (HttpContent content = new StringContent(body))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue(contentType)
                    {
                        CharSet = charSet
                    };
                    if (null != headers)
                    {
                        foreach (var item in headers)
                        {
                            content.Headers.Add(item.Key, item.Value);
                        }
                    }
                    response = httpClient.PostAsync(url, content).Result;
                }
                if (response != null && response.IsSuccessStatusCode)
                {
                    using (response)
                    {
                        return new Dictionary<ResponseCode, string>
                            {
                                { (ResponseCode)response.StatusCode, response.Content.ReadAsStringAsync().Result }
                            };
                    }
                }
            }
            catch (WebException ex)
            {
                return new Dictionary<ResponseCode, string>()
                {
                    {ResponseCode.SERVER_ERROR,ex.Message}
                };
            }
            return new Dictionary<ResponseCode, string> { { ResponseCode.BAD_REQUEST, "" } };
        }
    }
}