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
using System.Net.Http;

namespace AzureStorageWebService.Controllers
{
    public class TableServiceController : ApiController
    {
        public static string TableOperationErrorMessage = "Operation failed, please check your table name.";

        // GET: TableService
        public HttpResponseMessage GetAllTables(string accountName, string sasToken)
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
                return AzureStorageWebServiceUtil.ConstructHttpResponseUsingInstance(Request, tableNames);
            }
            catch (StorageException e)
            {
                return AzureStorageWebServiceUtil.DealWithTheStorageException(e, Request, TableOperationErrorMessage);
            }
        }

        //回头要改为post
        [HttpPost]
        public HttpResponseMessage CreateTable(string accountName, string sasToken, string createTableName)
        {
            try
            {
                AzureTableStorageAdapter tableStorageAdapter = new AzureTableStorageAdapter(accountName, AzureStorageWebServiceUtil.DecodeParamter(sasToken));
                var opResult = tableStorageAdapter.CreateTable(createTableName);
                return AzureStorageWebServiceUtil.ConstructHttpResponseUsingOperationResult(Request, opResult);
            }
            catch (StorageException e)
            {
                return AzureStorageWebServiceUtil.DealWithTheStorageException(e, Request, TableOperationErrorMessage);
            }
        }

        //回头改为HttpDelete
        [HttpDelete]
        public HttpResponseMessage DeleteTable(string accountName, string sasToken, string deleteTableName)
        {
            try
            {
                AzureTableStorageAdapter tableStorageAdapter= new AzureTableStorageAdapter(accountName, AzureStorageWebServiceUtil.DecodeParamter(sasToken));
                var opResult = tableStorageAdapter.DeleteTable(deleteTableName);
                return AzureStorageWebServiceUtil.ConstructHttpResponseUsingOperationResult(Request, opResult);
            }
            catch (StorageException e)
            {
                return AzureStorageWebServiceUtil.DealWithTheStorageException(e, Request, TableOperationErrorMessage);
            }
        }
    }
}