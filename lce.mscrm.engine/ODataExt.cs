/* file name：lce.mscrm.engine.ODataExt.cs
* author：lynx lynx.kor@163.com @ 2021/7/3 13:42:48
* copyright (c) 2021 Copyright@lynxce.com
* desc：
* > add description for ODataExt
* revision：
*
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json.Linq;

namespace lce.mscrm.engine
{
    /// <summary>
    /// action：ODataExt
    /// </summary>
    public static class ODataExt
    {
        /// <summary>
        /// Clones a HttpRequestMessage instance
        /// </summary>
        /// <param name="request">The HttpRequestMessage to clone.</param>
        /// <returns>A copy of the HttpRequestMessage</returns>
        public static HttpRequestMessage Clone(this HttpRequestMessage request)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri)
            {
                Content = request.Content.Clone(),
                Version = request.Version
            };
            foreach (KeyValuePair<string, object> prop in request.Properties)
            {
                clone.Properties.Add(prop);
            }
            foreach (KeyValuePair<string, IEnumerable<string>> header in request.Headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return clone;
        }

        /// <summary>
        /// Clones a HttpContent instance
        /// </summary>
        /// <param name="content">The HttpContent to clone</param>
        /// <returns>A copy of the HttpContent</returns>
        public static HttpContent Clone(this HttpContent content)
        {
            if (content == null) return null;

            HttpContent clone;
            switch (content)
            {
                case StringContent sc:
                    clone = new StringContent(sc.ReadAsStringAsync().Result);
                    break;

                default:
                    throw new Exception($"{content.GetType()} Content type not implemented for HttpContent.Clone extension method.");
            }

            clone.Headers.Clear();
            foreach (KeyValuePair<string, IEnumerable<string>> header in content.Headers)
            {
                clone.Headers.Add(header.Key, header.Value);
            }

            return clone;
        }

        /// <summary>
        /// Deletes an entity
        /// </summary>
        /// <param name="client"> </param>
        /// <param name="path">   The path to the resource to delete</param>
        /// <param name="headers">Any custom headers to control optional behaviors.</param>
        public static void Delete(this HttpClient client, string path, Dictionary<string, List<string>> headers = null)
        {
            client.DeleteAsync(path, headers).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Deletes an entity asychronously
        /// </summary>
        /// <param name="client"> </param>
        /// <param name="path">   The path to the resource to delete.</param>
        /// <param name="headers">Any custom headers to control optional behaviors.</param>
        /// <returns>Task</returns>
        public static async Task DeleteAsync(this HttpClient client, string path, Dictionary<string, List<string>> headers = null)
        {
            try
            {
                using (var message = new HttpRequestMessage(HttpMethod.Delete, new Uri(path)))
                {
                    if (headers != null)
                    {
                        foreach (KeyValuePair<string, List<string>> header in headers)
                        {
                            message.Headers.Add(header.Key, header.Value);
                        }
                    }

                    using (HttpResponseMessage response = await SendAsync(client, message))
                        response.Dispose();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets a typed response from a specified resource.
        /// </summary>
        /// <typeparam name="T">The type of response</typeparam>
        /// <param name="client"> </param>
        /// <param name="path">   The path to the resource.</param>
        /// <param name="headers">Any custom headers to control optional behaviors.</param>
        /// <returns>The typed response from the request.</returns>
        public static T Get<T>(this HttpClient client, string path, Dictionary<string, List<string>> headers = null)
        {
            return client.GetAsync<T>(path, headers).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Retrieves data from a specified resource.
        /// </summary>
        /// <param name="client"> </param>
        /// <param name="path">   The path to the resource</param>
        /// <param name="headers">Any custom headers to control optional behaviors.</param>
        /// <returns>The response from the request.</returns>
        public static JToken Get(this HttpClient client, string path, Dictionary<string, List<string>> headers = null)
        {
            return client.GetAsync(path, headers).GetAwaiter().GetResult(); ;
        }

        /// <summary>
        /// Gets a typed response from a specified resource asychronously
        /// </summary>
        /// <typeparam name="T">The type of resource</typeparam>
        /// <param name="client"> </param>
        /// <param name="path">   The path to the resource.</param>
        /// <param name="headers"></param>
        /// <returns>Any custom headers to control optional behaviors.</returns>
        public static async Task<T> GetAsync<T>(this HttpClient client, string path, Dictionary<string, List<string>> headers = null)
        {
            return (await client.GetAsync(path, headers)).ToObject<T>();
        }

        /// <summary>
        /// Retrieves data from a specified resource asychronously.
        /// </summary>
        /// <param name="client"> </param>
        /// <param name="path">   The path to the resource.</param>
        /// <param name="headers">Any custom headers to control optional behaviors.</param>
        /// <returns>The response to the request.</returns>
        public static async Task<JToken> GetAsync(this HttpClient client, string path, Dictionary<string, List<string>> headers = null)
        {
            try
            {
                using (var message = new HttpRequestMessage(HttpMethod.Get, path))
                {
                    if (headers != null)
                    {
                        foreach (KeyValuePair<string, List<string>> header in headers)
                        {
                            message.Headers.Add(header.Key, header.Value);
                        }
                    }

                    using (HttpResponseMessage response = await SendAsync(client, message, HttpCompletionOption.ResponseContentRead))
                    {
                        if (response.StatusCode != HttpStatusCode.NotModified)
                        {
                            return JToken.Parse(await response.Content.ReadAsStringAsync());
                        }
                    }
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="url">      </param>
        /// <param name="username"> </param>
        /// <param name="password"> </param>
        /// <param name="authority"></param>
        /// <param name="callerId"> </param>
        /// <param name="clientId"> </param>
        /// <param name="version">  </param>
        /// <returns></returns>
        public static HttpClient GetClient(string url, string username, string password
            , string authority, string clientId = "c7ea0955-0fcc-4a43-9d80-4093496f45e1"
            , string callerId = "", string version = "8.2")
        {
            HttpMessageHandler messageHandler = new OAuthMessageHandler(
                 new HttpClientHandler() { UseCookies = false }
                 , url, username, password, authority, clientId
                 );

            var webApiUrl = $"{url.TrimEnd('/')}/api/data/v{version}/";

            var httpClient = new HttpClient(messageHandler)
            {
                BaseAddress = new Uri(webApiUrl)
            };

            httpClient.Timeout = new TimeSpan(0, 2, 0);
            httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
            httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            if (!string.IsNullOrEmpty(callerId))
            {
                httpClient.DefaultRequestHeaders.Add("CallerObjectId", callerId);
            }
            return httpClient;
        }

        /// <summary>
        /// Sends a PATCH request to update a resource.
        /// </summary>
        /// <param name="client"> </param>
        /// <param name="uri">    </param>
        /// <param name="body">   The payload to send to update the resource.</param>
        /// <param name="headers">Any custom headers to control optional behaviors.</param>
        public static void Patch(this HttpClient client, Uri uri, object body, Dictionary<string, List<string>> headers = null)
        {
            client.PatchAsync(uri, body, headers).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Sends a PATCH request to update a resource.
        /// </summary>
        /// <param name="client"> </param>
        /// <param name="uri">    </param>
        /// <param name="body">   </param>
        /// <param name="headers"></param>
        public static void Patch(this HttpClient client, string url, object body, Dictionary<string, List<string>> headers = null)
        {
            client.PatchAsync(url, body, headers).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Sends a PATCH request to update a resource asynchronously
        /// </summary>
        /// <param name="client"> </param>
        /// <param name="url">    </param>
        /// <param name="body">   </param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static async Task PatchAsync(this HttpClient client, string url, object body, Dictionary<string, List<string>> headers = null)
        {
            var path = $"{client.BaseAddress.AbsoluteUri.TrimEnd('/')}/{url.TrimEnd('/')}"; // client.BaseAddress
            await client.PatchAsync(new Uri(path), body, headers);
        }

        /// <summary>
        /// Sends a PATCH request to update a resource asynchronously
        /// </summary>
        /// <param name="client"> </param>
        /// <param name="uri">    </param>
        /// <param name="body">   The payload to send to update the resource.</param>
        /// <param name="headers">Any custom headers to control optional behaviors.</param>
        /// <returns>Task</returns>
        public static async Task PatchAsync(this HttpClient client, Uri uri, object body, Dictionary<string, List<string>> headers = null)
        {
            try
            {
                using (var message = new HttpRequestMessage(new HttpMethod("PATCH"), uri))
                {
                    message.Content = new StringContent(JObject.FromObject(body).ToString());
                    message.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                    if (headers != null)
                    {
                        foreach (KeyValuePair<string, List<string>> header in headers)
                        {
                            message.Headers.Add(header.Key, header.Value);
                        }
                    }
                    using (HttpResponseMessage response = await SendAsync(client, message))
                        response.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Posts a payload to the specified resource.
        /// </summary>
        /// <param name="client"> </param>
        /// <param name="path">   The path to the resource</param>
        /// <param name="body">   The payload to send.</param>
        /// <param name="headers">Any headers to control optional behaviors.</param>
        /// <returns>The response from the request.</returns>
        public static JObject Post(this HttpClient client, string path, object body, Dictionary<string, List<string>> headers = null)
        {
            return client.PostAsync(path, body, headers).GetAwaiter().GetResult(); ;
        }

        /// <summary>
        /// Posts a payload to the specified resource asynchronously.
        /// </summary>
        /// <param name="client"> </param>
        /// <param name="path">   The path to the resource.</param>
        /// <param name="body">   The payload to send.</param>
        /// <param name="headers">Any headers to control optional behaviors.</param>
        /// <returns>The response from the request.</returns>
        public static async Task<JObject> PostAsync(this HttpClient client, string path, object body, Dictionary<string, List<string>> headers = null)
        {
            try
            {
                using (var message = new HttpRequestMessage(HttpMethod.Post, path))
                {
                    message.Content = new StringContent(JObject.FromObject(body).ToString());
                    message.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                    if (headers != null)
                    {
                        foreach (KeyValuePair<string, List<string>> header in headers)
                        {
                            message.Headers.Add(header.Key, header.Value);
                        }
                    }
                    using (HttpResponseMessage response = await SendAsync(client, message))
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        if (string.IsNullOrEmpty(content))
                        {
                            return null;
                        }
                        return JObject.Parse(content);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Creates an entity and returns the URI
        /// </summary>
        /// <param name="client">       </param>
        /// <param name="entitySetName">The entity set name of the entity to create.</param>
        /// <param name="body">         The JObject containing the data of the entity to create.</param>
        /// <returns>The Uri for the created entity record.</returns>
        public static Uri PostCreate(this HttpClient client, string entitySetName, object body)
        {
            try
            {
                return client.PostCreateAsync(entitySetName, body).GetAwaiter().GetResult();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Creates an entity asynchronously and returns the URI
        /// </summary>
        /// <param name="client">       </param>
        /// <param name="entitySetName">The entity set name of the entity to create.</param>
        /// <param name="body">         The JObject containing the data of the entity to create.</param>
        /// <returns>The Uri for the created entity record.</returns>
        public static async Task<Uri> PostCreateAsync(this HttpClient client, string entitySetName, object body)
        {
            try
            {
                using (var message = new HttpRequestMessage(HttpMethod.Post, entitySetName))
                {
                    message.Content = new StringContent(JObject.FromObject(body).ToString());
                    message.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                    using (HttpResponseMessage response = await SendAsync(client, message))
                        return new Uri(response.Headers.GetValues("OData-EntityId").FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Sends a POST to create a typed entity and retrieves the created entity.
        /// </summary>
        /// <typeparam name="T">
        /// The path to the entityset and any query string parameters to specify the properties to return.
        /// </typeparam>
        /// <param name="client"> </param>
        /// <param name="path">   The path to the entityset.</param>
        /// <param name="body">   The payload to send to create the entity record.</param>
        /// <param name="headers">Any headers to control optional behaviors.</param>
        /// <returns>The typed entity record created.</returns>
        public static T PostGet<T>(this HttpClient client, string path, object body, Dictionary<string, List<string>> headers = null)
        {
            return client.PostGetAsync<T>(path, body, headers).GetAwaiter().GetResult();
        }

        public static JObject PostGet(this HttpClient client, string path, object body, Dictionary<string, List<string>> headers = null)
        {
            return client.PostGetAsync(path, body, headers).GetAwaiter().GetResult();
        }

        public static async Task<T> PostGetAsync<T>(this HttpClient client, string path, object body, Dictionary<string, List<string>> headers = null)
        {
            return (await client.PostGetAsync(path, body, headers)).ToObject<T>();
        }

        /// <summary>
        /// Sents a POST to create an entity and retrieves the created entity asynchronously.
        /// </summary>
        /// <param name="client"> </param>
        /// <param name="path">   The path to the entityset.</param>
        /// <param name="body">   The payload to send to create the entity record.</param>
        /// <param name="headers">Any headers to control optional behaviors.</param>
        /// <returns>An object containing data for the created entity.</returns>
        public static async Task<JObject> PostGetAsync(this HttpClient client, string path, object body, Dictionary<string, List<string>> headers = null)
        {
            if (headers == null)
            {
                headers = new Dictionary<string, List<string>>();
            }
            headers.Add("Prefer", new List<string> { "return=representation" });
            return await client.PostAsync(path, body, headers);
        }

        /// <summary>
        /// Updates a property of an entity
        /// </summary>
        /// <param name="client">  </param>
        /// <param name="path">    The path to the entity.</param>
        /// <param name="property">The name of the property to update.</param>
        /// <param name="value">   The value to set.</param>
        public static void Put(this HttpClient client, string path, string property, string value)
        {
            client.PutAsync(path, property, value).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Used to update metadata
        /// </summary>
        /// <param name="client">      </param>
        /// <param name="path">        The path to the metadata resource.</param>
        /// <param name="metadataItem">The payload to send to update the metadata resource.</param>
        /// <param name="mergeLabels"> Whether any existing language labels should be merged.</param>
        public static void Put(this HttpClient client, string path, JObject metadataItem, bool mergeLabels)
        {
            client.PutAsync(path, metadataItem, mergeLabels).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Updates a property of an entity asychronously
        /// </summary>
        /// <param name="client">  </param>
        /// <param name="path">    The path to the entity.</param>
        /// <param name="property">The name of the property to update.</param>
        /// <param name="value">   The value to set.</param>
        /// <returns>Task</returns>
        public static async Task PutAsync(this HttpClient client, string path, string property, string value)
        {
            try
            {
                using (var message = new HttpRequestMessage(HttpMethod.Put, new Uri($"{path}/{property}")))
                {
                    var body = new JObject
                    {
                        ["value"] = value
                    };
                    message.Content = new StringContent(body.ToString());
                    message.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                    using (HttpResponseMessage response = await SendAsync(client, message))
                        response.Dispose();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Used to update metadata asychronously.
        /// </summary>
        /// <param name="client">      </param>
        /// <param name="path">        The path to the metadata resource.</param>
        /// <param name="metadataItem">The payload to send to update the metadata resource.</param>
        /// <param name="mergeLabels"> Whether any existing language labels should be merged.</param>
        public static async Task PutAsync(this HttpClient client, string path, JObject metadataItem, bool mergeLabels)
        {
            try
            {
                using (var message = new HttpRequestMessage(HttpMethod.Put, path))
                {
                    if (mergeLabels)
                    {
                        message.Headers.Add("MSCRM.MergeLabels", "true");
                        message.Headers.Add("Consistency", "Strong");
                    }

                    message.Content = new StringContent(metadataItem.ToString());
                    message.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                    using (HttpResponseMessage response = await SendAsync(client, message))
                        response.Dispose();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Parses the Web API error
        /// </summary>
        /// <param name="response">The response that failed.</param>
        /// <returns></returns>
        private static ServiceException ParseError(HttpResponseMessage response)
        {
            try
            {
                int code = 0;
                string message = "no content returned",
                       content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                if (content.Length > 0)
                {
                    var errorObject = JObject.Parse(content);
                    message = errorObject["error"]["message"].Value<string>();
                    code = Convert.ToInt32(errorObject["error"]["code"].Value<string>(), 16);
                }
                int statusCode = (int)response.StatusCode;
                string reasonPhrase = response.ReasonPhrase;

                return new ServiceException(code, statusCode, reasonPhrase, message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Sends all requests with retry capabilities
        /// </summary>
        /// <param name="client">              </param>
        /// <param name="request">             The request to send</param>
        /// <param name="httpCompletionOption">
        /// Indicates if HttpClient operations should be considered completed either as soon as a
        /// response is available, or after reading the entire response message including the content.
        /// </param>
        /// <param name="retryCount">          The number of retry attempts</param>
        /// <returns>The response for the request.</returns>
        private static async Task<HttpResponseMessage> SendAsync(HttpClient client,
            HttpRequestMessage request,
            HttpCompletionOption httpCompletionOption = HttpCompletionOption.ResponseHeadersRead,
            int retryCount = 0)
        {
            HttpResponseMessage response;
            try
            {
                //The request is cloned so it can be sent again.
                response = await client.SendAsync(request.Clone(), httpCompletionOption);
            }
            catch (Exception)
            {
                throw;
            }

            if (!response.IsSuccessStatusCode)
            {
                if ((int)response.StatusCode != 429)
                {
                    //Not a service protection limit error
                    throw ParseError(response);
                }
                else
                {
                    // Give up re-trying if exceeding the maxRetries
                    if (++retryCount >= 5)
                    {
                        throw ParseError(response);
                    }

                    int seconds;
                    //Try to use the Retry-After header value if it is returned.
                    if (response.Headers.Contains("Retry-After"))
                    {
                        seconds = int.Parse(response.Headers.GetValues("Retry-After").FirstOrDefault());
                    }
                    else
                    {
                        //Otherwise, use an exponential backoff strategy
                        seconds = (int)Math.Pow(2, retryCount);
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(seconds));

                    return await SendAsync(client, request, httpCompletionOption, retryCount);
                }
            }
            else
            {
                return response;
            }
        }

        public class OAuthMessageHandler : DelegatingHandler
        {
            private readonly AuthenticationContext _authContext;
            private readonly string _clientId;
            private readonly UserCredential _credential = null;
            private readonly string _redirectUrl;
            private readonly string _url;

            public OAuthMessageHandler(HttpMessageHandler innerHandler, string url, string username, string password
                , string authority, string clientId, string redirectUrl = "")
                : base(innerHandler)
            {
                _url = url;
                _clientId = clientId;
                _redirectUrl = redirectUrl;
                _credential = new UserCredential(username, password);
                _authContext = new AuthenticationContext(authority, false);
            }

            /// <summary>
            /// Overrides the default HttpClient.SendAsync operation so that authentication can be done.
            /// </summary>
            /// <param name="request">          The request to send</param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            protected override Task<HttpResponseMessage> SendAsync(
                      HttpRequestMessage request, CancellationToken cancellationToken)
            {
                try
                {
                    request.Headers.Authorization = GetAuthHeader();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return base.SendAsync(request, cancellationToken);
            }

            /// <summary>
            /// Will refresh the ADAL AccessToken when it expires.
            /// </summary>
            /// <returns></returns>
            private AuthenticationHeaderValue GetAuthHeader()
            {
                var authResult = _authContext.AcquireTokenAsync(_url, _clientId, _credential).Result;
                return new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
            }
        }

        /// <summary>
        /// An exception that captures data returned by the Web API
        /// </summary>
        public class ServiceException : Exception
        {
            /// <summary>
            /// </summary>
            /// <param name="errorcode">   </param>
            /// <param name="statuscode">  </param>
            /// <param name="reasonphrase"></param>
            /// <param name="message">     </param>
            public ServiceException(int errorcode, int statuscode, string reasonphrase, string message) : base(message)
            {
                ErrorCode = errorcode;
                StatusCode = statuscode;
                ReasonPhrase = reasonphrase;
            }

            /// <summary>
            /// </summary>
            public int ErrorCode { get; private set; }

            /// <summary>
            /// </summary>
            public string ReasonPhrase { get; private set; }

            /// <summary>
            /// </summary>
            public int StatusCode { get; private set; }
        }
    }
}