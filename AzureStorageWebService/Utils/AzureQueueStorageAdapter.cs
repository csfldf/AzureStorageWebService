using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;

namespace AzureStorageWebService.Utils
{
    public class AzureQueueStorageAdapter
    {
        private CloudQueueClient cloudQueueClient;

        public AzureQueueStorageAdapter(string accountName, string sasToken)
        {
            StorageCredentials accountSAS = new StorageCredentials(sasToken);
            CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(accountSAS, blobEndpoint: null, queueEndpoint: new Uri("https://" + accountName + ".queue.core.windows.net"), tableEndpoint: null, fileEndpoint: null);
            cloudQueueClient = cloudStorageAccount.CreateCloudQueueClient();
        }

        public IEnumerable<CloudQueue> GetAllQueues()
        {
            return cloudQueueClient.ListQueues();
        }

        public IEnumerable<CloudQueueMessage> GetMessages(string queueName, int messageCount)
        {
            CloudQueue cloudQueue = cloudQueueClient.GetQueueReference(queueName);

            //回头测ApproximateMessageCount是不是要用FetchAttributes才能得到
            //if (cloudQueue.ApproximateMessageCount == null || cloudQueue.ApproximateMessageCount == 0)
            //{
            //    return new List<CloudQueueMessage>();
            //}

            IAsyncResult result = cloudQueue.BeginGetMessages(messageCount, null, null);
            AzureStorageWebServiceUtil.WaitingForAsyncResult(result);
            return cloudQueue.EndGetMessages(result);
        }

        public Tuple<bool, string> DeleteQueue(string queueName)
        {
            try
            {
                CloudQueue cloudQueue = cloudQueueClient.GetQueueReference(queueName);
                IAsyncResult result = cloudQueue.BeginDeleteIfExists(null, null);
                AzureStorageWebServiceUtil.WaitingForAsyncResult(result);
                bool opResult = cloudQueue.EndDeleteIfExists(result);

                if (opResult)
                {
                    return Tuple.Create<bool, string>(true, "Delete queue successfully");
                }
                else
                {
                    return Tuple.Create<bool, string>(false, "Delete queue unsuccessfully");
                }
            }
            catch (StorageException)
            {
                return Tuple.Create<bool, string>(false, "Delete queue unsuccessfully");
            }
        }

        public Tuple<bool, string> CreateQueue(string queueName)
        {
            try
            {
                CloudQueue cloudQueue = cloudQueueClient.GetQueueReference(queueName);
                IAsyncResult result = cloudQueue.BeginCreateIfNotExists(null, null);
                AzureStorageWebServiceUtil.WaitingForAsyncResult(result);
                bool opResult = cloudQueue.EndCreateIfNotExists(result);

                if (opResult)
                {
                    return Tuple.Create<bool, string>(true, "Create queue successfully");
                }
                else
                {
                    return Tuple.Create<bool, string>(false, "Create queue unsuccessfully");
                }
            }
            catch (StorageException)
            {
                return Tuple.Create<bool, string>(false, "Create queue unsuccessfully");
            }
        }

        public Tuple<bool, string> EnqueueMessage(string queueName, string message)
        {
            try
            {
                CloudQueue cloudQueue = cloudQueueClient.GetQueueReference(queueName);
                cloudQueue.AddMessage(new CloudQueueMessage(message), new TimeSpan(TimeSpan.TicksPerDay));
                return Tuple.Create<bool, string>(true, string.Format("Enqueue message to queue {0} successfully", queueName));
            }
            catch (StorageException)
            {
                return Tuple.Create<bool, string>(false, "Enqueue message to queue {0} unsuccessfully");
            }
        }

    }
}
