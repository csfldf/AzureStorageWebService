using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using AzureStorageWebService.Utils;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using AzureStorageWebService.ParamsModel.Table;
using AzureStorageWebService.ResponseMessage;

namespace AzureStorageWebService.Controllers
{
    public class TableServiceController : ApiController
    {
        public static string TableOperationErrorMessage = "Operation failed, please check your table name";

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

                return AzureStorageWebServiceUtil.ConstructHttpResponseUsingInstance(Request, new ResultWithDataResponse<List<String>>(true, tableNames));
            }
            catch (StorageException e)
            {
                return AzureStorageWebServiceUtil.DealWithTheStorageException(e, Request, TableOperationErrorMessage);
            }
        }

        //回头要改为post
        [HttpPost]
        public HttpResponseMessage CreateTable([FromBody]TableOperationParamsModel parameters)
        {
            try
            {
                AzureTableStorageAdapter tableStorageAdapter = new AzureTableStorageAdapter(parameters.AccountName, AzureStorageWebServiceUtil.DecodeParamter(parameters.SasToken));
                var opResult = tableStorageAdapter.CreateTable(parameters.TableName);
                return AzureStorageWebServiceUtil.ConstructHttpResponseUsingOperationResult(Request, opResult);
            }
            catch (StorageException e)
            {
                return AzureStorageWebServiceUtil.DealWithTheStorageException(e, Request, TableOperationErrorMessage);
            }
        }

        //回头改为HttpDelete
        [HttpDelete]
        public HttpResponseMessage DeleteTable(string accountName, string sasToken, string tableName)
        {
            try
            {
                AzureTableStorageAdapter tableStorageAdapter= new AzureTableStorageAdapter(accountName, AzureStorageWebServiceUtil.DecodeParamter(sasToken));
                var opResult = tableStorageAdapter.DeleteTable(tableName);
                return AzureStorageWebServiceUtil.ConstructHttpResponseUsingOperationResult(Request, opResult);
            }
            catch (StorageException e)
            {
                return AzureStorageWebServiceUtil.DealWithTheStorageException(e, Request, TableOperationErrorMessage);
            }
        }
    }
}