using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using System.Text;

namespace AzureStorageWebService.Utils
{
    public static class AzureStorageWebServiceUtil
    {
        public static void WaitingForAsyncResult(IAsyncResult result)
        {
            while (result.IsCompleted != true)
            {
                Thread.Sleep(AzureStorageWebServiceConstantValue.SleepTime);
            }
        }

        public static HttpResponseException ConstructHttpResponseException(HttpStatusCode statusCode, string reasonPhrase, string responseContent)
        {
            var resp = new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(responseContent, Encoding.UTF8),
                ReasonPhrase = reasonPhrase
            };
            return new HttpResponseException(resp);
        }

        public static string DecodeParamter(string paramter)
        {
            return paramter;
        }
    }
}