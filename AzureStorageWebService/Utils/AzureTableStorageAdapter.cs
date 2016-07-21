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

        public Tuple<bool, string> CreateTable(string tableName)
        {
            CloudTable cloudTable = cloudTableClient.GetTableReference(tableName);

            IAsyncResult result = cloudTable.BeginCreateIfNotExists(null, null);
            AzureStorageWebServiceUtil.WaitingForAsyncResult(result);
            if (cloudTable.EndCreateIfNotExists(result))
            {
                return Tuple.Create<bool, string>(true, string.Format("Create table {0} successfully", tableName));
            }
            else
            {
                return Tuple.Create<bool, string>(false, string.Format("Create table {0} unsuccessfully", tableName));
            }
        }

        public Tuple<bool, string> DeleteTable(string tableName)
        {
            CloudTable cloudTable = cloudTableClient.GetTableReference(tableName);
            IAsyncResult result = cloudTable.BeginDeleteIfExists(null, null);
            AzureStorageWebServiceUtil.WaitingForAsyncResult(result);
            if (cloudTable.EndDeleteIfExists(result))
            {
                return Tuple.Create<bool, string>(true, string.Format("Delete table {0} successfully", tableName));
            }
            else
            {
                return Tuple.Create<bool, string>(false, string.Format("Delete table {0} unsuccessfully", tableName));
            }
        }
    }
}