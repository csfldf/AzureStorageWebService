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
        public static string QueueOperationErrorMessage = "Operation failed, please check your queue name.";

        public HttpResponseMessage GetAllQueues(string accountName, string sasToken)
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

                return AzureStorageWebServiceUtil.ConstructHttpResponseUsingInstance(Request, queues);
            }
            catch (StorageException e)
            {
                return AzureStorageWebServiceUtil.DealWithTheStorageException(e, Request, QueueOperationErrorMessage);
            }
        }

        public HttpResponseMessage GetMessagesInSpecificQueue(string accountName, string sasToken, string queueName, int messageCount)
        {
            try
            {
                if (messageCount == 0)
                {
                    return AzureStorageWebServiceUtil.ConstructHttpResponseUsingInstance(Request, new List<CloudQueueMessage>());
                }

                AzureQueueStorageAdapter queueStorageAdapter = new AzureQueueStorageAdapter(accountName, AzureStorageWebServiceUtil.DecodeParamter(sasToken));
                return AzureStorageWebServiceUtil.ConstructHttpResponseUsingInstance(Request, queueStorageAdapter.GetMessages(queueName, messageCount));
            }
            catch (StorageException e)
            {
                return AzureStorageWebServiceUtil.DealWithTheStorageException(e, Request, QueueOperationErrorMessage);
            }
        }


        //回头要改成Post
        [HttpPost]
        public HttpResponseMessage CreateQueue(string accountName, string sasToken, string queueName)
        {
            try
            {
                AzureQueueStorageAdapter queueStorageAdapter = new AzureQueueStorageAdapter(accountName, AzureStorageWebServiceUtil.DecodeParamter(sasToken));
                return AzureStorageWebServiceUtil.ConstructHttpResponseUsingOperationResult(Request, queueStorageAdapter.CreateQueue(queueName));
            }
            catch (StorageException e)
            {
                return AzureStorageWebServiceUtil.DealWithTheStorageException(e, Request, QueueOperationErrorMessage);
            }
        }

        //回头要改成Delete
        [HttpDelete]
        public HttpResponseMessage DeleteQueue(string accountName, string sasToken, string queueName)
        {
            try
            {
                AzureQueueStorageAdapter queueStorageAdapter = new AzureQueueStorageAdapter(accountName, AzureStorageWebServiceUtil.DecodeParamter(sasToken));
                return AzureStorageWebServiceUtil.ConstructHttpResponseUsingOperationResult(Request, queueStorageAdapter.DeleteQueue(queueName));
            }
            catch (StorageException e)
            {
                return AzureStorageWebServiceUtil.DealWithTheStorageException(e, Request, QueueOperationErrorMessage);
            }
        }

        //回头要改成Post
        [HttpPost]
        public HttpResponseMessage EnqueueMessage(string accountName, string sasToken, string queueName, string message)
        {
            try
            {
                AzureQueueStorageAdapter queueStorageAdapter = new AzureQueueStorageAdapter(accountName, AzureStorageWebServiceUtil.DecodeParamter(sasToken));
                return AzureStorageWebServiceUtil.ConstructHttpResponseUsingOperationResult(Request, queueStorageAdapter.EnqueueMessage(queueName, message));
            }
            catch (StorageException e)
            {
                return AzureStorageWebServiceUtil.DealWithTheStorageException(e, Request, QueueOperationErrorMessage);
            }
        }
    }
}
