/* file name：${namespace}.HttpClientExt.cs
* author：lynx <lynx.kor@163.com> @ 2020/4/11 0:45
* copyright (c) 2020 Copyright@lynxce.com
* desc：
* > add description for HttpClientExt
* revision：
*
*/

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using lce.provider.Enums;

namespace lce.provider
{
    /// <summary>
    /// HttpClientExt
    /// </summary>
    public static class HttpClientExt
    {
        /// <summary>
        /// POST
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
            var body = "";
            if (null != param && param.Count > 0)
            {
                body = param.ToJson();
            }
            return Post(url, body, headers, "", "", contentType, charSet);
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="url">        </param>
        /// <param name="body">       </param>
        /// <param name="headers">    </param>
        /// <param name="token">      </param>
        /// <param name="scheme">     </param>
        /// <param name="contentType"></param>
        /// <param name="charSet">    </param>
        /// <returns></returns>
        public static Dictionary<ResponseCode, string> Post(string url, string body
            , Dictionary<string, string> headers
            , string token = "", string scheme = "Bearer"
            , string contentType = "application/json", string charSet = "utf-8")
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    HttpResponseMessage response = null;
                    using (HttpContent content = new StringContent(body))
                    {
                        content.Headers.ContentType = new MediaTypeHeaderValue(contentType)
                        {
                            CharSet = charSet
                        };
                        if (!string.IsNullOrEmpty(token))
                        {
                            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, token);
                        }
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
            }
            catch (Exception ex)
            {
                var msg = new
                {
                    url,
                    headers,
                    auth = $"{scheme} {token}",
                    body,
                    err = ex.Message
                }.ToJson();
                LogExt.e(msg, ex);
                return new Dictionary<ResponseCode, string> { { ResponseCode.SERVER_ERROR, msg } };
            }
            return new Dictionary<ResponseCode, string> { { ResponseCode.BAD_REQUEST, $"{url};{body}" } };
        }

        /// <summary>
        /// GET
        /// </summary>
        /// <param name="url">        </param>
        /// <param name="headers">    </param>
        /// <param name="token">      </param>
        /// <param name="scheme">     </param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static Dictionary<ResponseCode, string> Get(string url, Dictionary<string, string> headers
            , string token = "", string scheme = "Bearer", string contentType = "application/json")
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                    if (!string.IsNullOrEmpty(token))
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, token);
                    }
                    if (null != headers)
                    {
                        foreach (var item in headers)
                        {
                            httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
                        }
                    }
                    var response = httpClient.GetAsync(url).Result;
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
            }
            catch (Exception ex)
            {
                var msg = new
                {
                    url,
                    headers,
                    auth = $"{scheme} {token}",
                    err = ex.Message
                }.ToJson();
                LogExt.e(msg, ex);
                return new Dictionary<ResponseCode, string> { { ResponseCode.SERVER_ERROR, msg } };
            }
            return new Dictionary<ResponseCode, string> { { ResponseCode.BAD_REQUEST, $"{url}" } };
        }
    }
}