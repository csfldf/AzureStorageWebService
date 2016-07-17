using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;

namespace AzureStorageWebService.Utils
{
    public class AzureTableStorageAdapter
    {
        private CloudTableClient cloudTableClient;

        public AzureTableStorageAdapter(string accountName, string sasToken)
        {
            StorageCredentials accountSAS = new StorageCredentials(sasToken);
            CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(accountSAS, blobEndpoint: null, queueEndpoint: null, tableEndpoint: new Uri("https://" + accountName + ".table.core.windows.net"), fileEndpoint: null);
            cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
        }

        public IEnumerable<CloudTable> GetAllTables()
        {
            return cloudTableClient.ListTables();
        }

        public void ListTable(string tableName)
        {
            CloudTable cloudTable = cloudTableClient.GetTableReference(tableName);
            //看看能不能ListAllPartitions
        }

        public bool CreateTable(string tableName)
        {
            CloudTable cloudTable = cloudTableClient.GetTableReference(tableName);

            IAsyncResult result = cloudTable.BeginCreateIfNotExists(null, null);
            AzureStorageWebServiceUtil.WaitingForAsyncResult(result);
            return cloudTable.EndCreateIfNotExists(result);
        }

        public bool DeleteTable(string tableName)
        {
            CloudTable cloudTable = cloudTableClient.GetTableReference(tableName);
            IAsyncResult result = cloudTable.BeginDeleteIfExists(null, null);
            AzureStorageWebServiceUtil.WaitingForAsyncResult(result);
            return cloudTable.EndDeleteIfExists(result);
        }
    }
}