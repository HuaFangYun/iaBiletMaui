using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using iaBilet.Core.Lib;

namespace iaBilet.Core.Services
{
    public enum RequestMethod
    {
        Get = 0, Post = 1, Put = 2, Delete = 3, DeleteAll = 4
    }
    public class RestService : INotifyObject
    {
        protected HttpClient Client = new HttpClient();
        protected Exception LastException;

        public virtual Task<RestServiceResponse> FetchPost(string absoluteUrl, Dictionary<string, object> parameters = null, Dictionary<string, object> urlParams = null)
        {
            Uri uri = GetUri(absoluteUrl, urlParams);
            return Fetch(uri, parameters);
        }

        public Task<RestServiceResponse> FetchDelete(string absoluteUrl, Dictionary<string, object> parameters = null)
        {
            Uri uri = GetUri(absoluteUrl, parameters);
            return Fetch(uri, null, RequestMethod.Delete);
        }

        public Task<RestServiceResponse> FetchGet(string absoluteUrl, Dictionary<string, object> parameters = null)
        {
            Uri uri = GetUri(absoluteUrl, parameters);
            return Fetch(uri, null, RequestMethod.Get);
        }
        protected Task<RestServiceResponse> Fetch(Uri uri, object parameters = null, RequestMethod method = RequestMethod.Post)
        {
            return Task.Run(async () =>
            {
                Log.WriteLine(string.Format("Url:{0} params {1}", uri.AbsoluteUri , JsonConvert.SerializeObject(parameters)));
                RestServiceResponse responseObject = new RestServiceResponse();
                try
                {
                    parameters = parameters == null ? new Dictionary<string, string>() : parameters;
                    using (var content = new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json"))
                    {

                        HttpResponseMessage response;
                        if (method == RequestMethod.Delete)
                        {
                            response = await Client.DeleteAsync(uri);
                        }
                        else if (method == RequestMethod.Get)
                        {
                            response = await Client.GetAsync(uri);
                        }
                        else
                        {
                            response = await Client.PostAsync(uri, content);
                        }
                        Log.WriteLine("response.StatusCode = " + response.StatusCode);
                        responseObject.StatusCode = response.StatusCode;
                        responseObject.IsSuccessStatusCode = response.IsSuccessStatusCode;
                        if (response.StatusCode == HttpStatusCode.NoContent)
                        {
                            responseObject.Error = null;
                            responseObject.ResponseString = string.Empty;
                            return responseObject;
                        }
                        using (HttpContent httpcontent = response.Content)
                        {
                            string responseString = await httpcontent.ReadAsStringAsync();
                            Log.WriteLine("response: " + responseString);
                            
                            JToken token = JsonConvert.DeserializeObject<JToken>(responseString);
                            switch (token.Type)
                            {
                                case JTokenType.Array:
                                    responseObject.ResponseString = responseString;
                                    break;
                                case JTokenType.Object:
                                    if (token["error"] != null || !responseObject.IsSuccessStatusCode)
                                    {
                                        responseObject = JsonConvert.DeserializeObject<RestServiceResponse>(responseString);
                                        responseObject.ResponseString = responseString;
                                        responseObject.IsSuccessStatusCode = false;
                                    }
                                    else
                                    {
                                        responseObject.ResponseString = responseString;
                                    }
                                    break;
                            }
                            
                            return responseObject;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex);
                    responseObject = new RestServiceResponse();
                    responseObject.Error = "503";
                    responseObject.ErrorMessage = ex.Message;
                    responseObject.ResponseString = ex.ToString();
                    LastException = ex;
                    return responseObject;
                }
            });
        }

        public static Uri GetUri(string absoluteUrl, Dictionary<string, object> parameters = null)
        {
            if (parameters == null)
            {
                parameters = new Dictionary<string, object>();
            }
            parameters["_format"] = "json";
            string url = string.Format("{0}{1}{2}", absoluteUrl, parameters.Count() == 0 ? "" : absoluteUrl.IndexOf("?") >= 0 ? "&" : "?", string.Join("&", parameters.Select(kvp => $"{kvp.Key}={HttpUtility.UrlEncode(kvp.Value.ToString())}")));
            return new Uri(url);
        }
    }
}
