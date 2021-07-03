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
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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

                    HttpResponseMessage response = await client.SendAsync(message);
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

                    using (HttpResponseMessage response = await client.SendAsync(message, HttpCompletionOption.ResponseContentRead))
                    {
                        if (response.StatusCode != HttpStatusCode.NotModified)
                        {
                            return JToken.Parse(await response.Content.ReadAsStringAsync());
                        }
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="url">     </param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="callerId"></param>
        /// <param name="clientId"></param>
        /// <param name="version"> </param>
        /// <returns></returns>
        public static HttpClient GetClient(string url, string username, string password, string clientId = "c7ea0955-0fcc-4a43-9d80-4093496f45e1", string callerId = "", string version = "8.2")
        {
            var webApiUrl = $"{url.TrimEnd('/')}/api/data/v{version}/";
            var userCredential = new UserCredential(username, password);
            var authParameters = AuthenticationParameters.CreateFromResourceUrlAsync(new Uri(webApiUrl)).Result;
            var authContext = new AuthenticationContext(authParameters.Authority, false);
            var authResult = authContext.AcquireToken(url, clientId, userCredential);
            var authHeader = new AuthenticationHeaderValue("Bearer", authResult.AccessToken);

            var client = new HttpClient
            {
                BaseAddress = new Uri(webApiUrl),
                Timeout = new TimeSpan(0, 2, 0)
            };
            client.DefaultRequestHeaders.Authorization = authHeader;
            client.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
            client.DefaultRequestHeaders.Add("OData-Version", "4.0");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (!string.IsNullOrEmpty(callerId))
            {
                client.DefaultRequestHeaders.Add("CallerObjectId", callerId);
            }
            return client;
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
                    using (HttpResponseMessage response = await client.SendAsync(message))
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

        /// <summary>
        /// Sents a POST to create an entity and retrieves the created entity.
        /// </summary>
        /// <param name="client"> </param>
        /// <param name="path">   
        /// The path to the entityset and any query string parameters to specify the properties to return.
        /// </param>
        /// <param name="body">   The payload to send to create the entity record.</param>
        /// <param name="headers">Any headers to control optional behaviors.</param>
        /// <returns>An object containing data for the created entity.</returns>
        public static JObject PostGet(this HttpClient client, string path, object body, Dictionary<string, List<string>> headers = null)
        {
            return client.PostGetAsync(path, body, headers).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Sents a POST to create a typed entity and retrieves the created entity asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of entity to create and retrieve.</typeparam>
        /// <param name="client"> </param>
        /// <param name="path">   
        /// The path to the entityset and any query string parameters to specify the properties to return.
        /// </param>
        /// <param name="body">   The payload to send to create the entity record.</param>
        /// <param name="headers">Any headers to control optional behaviors.</param>
        /// <returns>The typed entity record created.</returns>
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
                    HttpResponseMessage response = await client.SendAsync(message);
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
                    HttpResponseMessage response = await client.SendAsync(message);
                    response.Dispose();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}