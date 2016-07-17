using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using System.Web.Http;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Auth;
using AzureStorageWebService.Utils;
using System.Net;

namespace AzureStorageWebService.Controllers
{
    public class TableServiceController : ApiController
    {
        // GET: TableService
        public IEnumerable<String> GetAllTables(string accountName, string sasToken)
        {
            try
            {
                AzureTableStorageAdapter tableStorageAdapter = new AzureTableStorageAdapter(accountName, AzureStorageWebServiceUtil.DecodeParamter(sasToken));
                IEnumerable<CloudTable> tables = tableStorageAdapter.GetAllTables();
                List<String> tableNames = new List<string>(tables.Count());

                foreach (CloudTable table in tables)
                {
                    tableNames.Add(table.Name);
                }

                return tableNames;
            }
            catch (StorageException)
            {
                throw AzureStorageWebServiceUtil.ConstructHttpResponseException(HttpStatusCode.Unauthorized, "accountName or sasToken error", "accountName or sasToken error");
            }
        }

        //回头要改为post
        [HttpGet]
        public bool CreateTable(string accountName, string sasToken, string createTableName)
        {
            try
            {
                AzureTableStorageAdapter tableStorageAdapter = new AzureTableStorageAdapter(accountName, AzureStorageWebServiceUtil.DecodeParamter(sasToken));
                return tableStorageAdapter.CreateTable(createTableName);
            }
            catch (StorageException)
            {
                throw AzureStorageWebServiceUtil.ConstructHttpResponseException(HttpStatusCode.Unauthorized, "accountName or sasToken error", "accountName or sasToken error");
            }
        }

        //回头改为HttpDelete
        [HttpGet]
        public bool DeleteTable(string accountName, string sasToken, string deleteTableName)
        {
            try
            {
                AzureTableStorageAdapter tableStorageAdapter= new AzureTableStorageAdapter(accountName, AzureStorageWebServiceUtil.DecodeParamter(sasToken));
                return tableStorageAdapter.DeleteTable(deleteTableName);
            }
            catch (StorageException)
            {
                throw AzureStorageWebServiceUtil.ConstructHttpResponseException(HttpStatusCode.Unauthorized, "accountName or sasToken error", "accountName or sasToken error");
            }
        }
    }
}