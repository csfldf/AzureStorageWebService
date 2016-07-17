using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AzureStorageWebService.Utils;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage;

namespace AzureStorageWebService.Controllers
{
    public class QueueServiceController : ApiController
    {
        public IEnumerable<string> GetAllQueues(string accountName, string sasToken)
        {
            try
            {
                AzureQueueStorageAdapter queueStorageAdapter = new AzureQueueStorageAdapter(accountName, AzureStorageWebServiceUtil.DecodeParamter(sasToken));
                IEnumerable<CloudQueue> queues = queueStorageAdapter.GetAllQueues();

                List<string> queueNames = new List<string>(queues.Count());
                foreach (CloudQueue queue in queues)
                {
                    queueNames.Add(queue.Name);
                }

                return queueNames;
            }
            catch (StorageException)
            {
                throw AzureStorageWebServiceUtil.ConstructHttpResponseException(HttpStatusCode.Unauthorized, "accountName or sasToken error", "accountName or sasToken error");
            }
        }

        public IEnumerable<CloudQueueMessage> GetMessagesInSpecificQueue(string accountName, string sasToken, string queueName, int messageCount)
        {
            try
            {
                if (messageCount == 0)
                {
                    return new List<CloudQueueMessage>();
                }

                AzureQueueStorageAdapter queueStorageAdapter = new AzureQueueStorageAdapter(accountName, AzureStorageWebServiceUtil.DecodeParamter(sasToken));
                return queueStorageAdapter.GetMessages(queueName, messageCount);
            }
            catch (StorageException)
            {
                throw AzureStorageWebServiceUtil.ConstructHttpResponseException(HttpStatusCode.Unauthorized, "accountName, sasToken or queueName error", "accountName, sasToken or queueName error");
            }
        }


        //回头要改成Post
        //[HttpGet]
        //public bool CreateQueue(string accountName, string sasToken, string queueName)
        //{
        //    try
        //    {
        //        AzureQueueStorageAdapter queueStorageAdapter = new AzureQueueStorageAdapter(accountName, AzureStorageWebServiceUtil.DecodeParamter(sasToken));
        //        return queueStorageAdapter.CreateQueue(queueName);
        //    }
        //    catch (StorageException)
        //    {
        //        throw AzureStorageWebServiceUtil.ConstructHttpResponseException(HttpStatusCode.Unauthorized, "accountName or sasToken error", "accountName or sasToken error");
        //    }
        //}

        //回头要改成Delete
        //[HttpGet]
        //public bool DeleteQueue(string accountName, string sasToken, string queueName)
        //{
        //    try
        //    {
        //        AzureQueueStorageAdapter queueStorageAdapter = new AzureQueueStorageAdapter(accountName, AzureStorageWebServiceUtil.DecodeParamter(sasToken));
        //        return queueStorageAdapter.DeleteQueue(queueName);
        //    }
        //    catch (StorageException)
        //    {
        //        throw AzureStorageWebServiceUtil.ConstructHttpResponseException(HttpStatusCode.Unauthorized, "accountName, sasToken or queueName error", "accountName, sasToken or queueName error");
        //    }
        //}

        //回头要改成Post
        [HttpGet]
        public bool EnqueueMessage(string accountName, string sasToken, string queueName, string message)
        {
            try
            {
                AzureQueueStorageAdapter queueStorageAdapter = new AzureQueueStorageAdapter(accountName, AzureStorageWebServiceUtil.DecodeParamter(sasToken));
                return queueStorageAdapter.EnqueueMessage(queueName, message);
            }
            catch (StorageException)
            {
                throw AzureStorageWebServiceUtil.ConstructHttpResponseException(HttpStatusCode.Unauthorized, "accountName, sasToken or queueName error", "accountName, sasToken or queueName error");
            }
        }
    }
}
