using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AzureStorageWebService.Utils;
using Microsoft.WindowsAzure.Storage.File;
using Microsoft.WindowsAzure.Storage;
using AzureStorageWebService.ResponseMessage;
using AzureStorageWebService.Exceptions;

namespace AzureStorageWebService.Controllers
{
    public class FileServiceController : ApiController
    {
        public static string FileOperationErrorMessage = "Operation failed, please check your file share and file names.";

        public HttpResponseMessage GetAllFileShares(string accountName, string sasToken)
        {
            try
            {
                AzureFileStorageAdapter fileStorageAdapter = new AzureFileStorageAdapter(accountName, AzureStorageWebServiceUtil.DecodeParamter(sasToken));
                IEnumerable<CloudFileShare> fileShares = fileStorageAdapter.GetAllFileShares();

                List<string> fileSharesNames = new List<string>(fileShares.Count());
                foreach (CloudFileShare fileShare in fileShares)
                {
                    fileSharesNames.Add(fileShare.Name);
                }

                return AzureStorageWebServiceUtil.ConstructHttpResponseUsingInstance(Request, fileSharesNames);
            }
            catch (StorageException e)
            {
                return AzureStorageWebServiceUtil.DealWithTheStorageException(e, Request, FileOperationErrorMessage);
            }
        }

        public HttpResponseMessage GetRootDirectoryContentInFileShare(string accountName, string sasToken, string fileShareName)
        {
            try
            {
                AzureFileStorageAdapter fileStorageAdapter = new AzureFileStorageAdapter(accountName, AzureStorageWebServiceUtil.DecodeParamter(sasToken));
                return AzureStorageWebServiceUtil.ConstructHttpResponseUsingInstance(Request, fileStorageAdapter.ListRootDirectoryInFileShare(fileShareName));
            }
            catch (StorageException e)
            {
                return AzureStorageWebServiceUtil.DealWithTheStorageException(e, Request, FileOperationErrorMessage);
            }
        }

        public HttpResponseMessage GetSpecificDirectoryContentInFileShare(string accountName, string sasToken, string fileShareName, string absolutePath)
        {
            try
            {
                AzureFileStorageAdapter fileStorageAdapter = new AzureFileStorageAdapter(accountName, AzureStorageWebServiceUtil.DecodeParamter(sasToken));
                return AzureStorageWebServiceUtil.ConstructHttpResponseUsingInstance(Request, fileStorageAdapter.ListRootDirectoryInFileShare(fileShareName));
            }
            catch (OperationCanceledException e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new BoolResultOpResponse(false, e.Message));
            }
            catch (StorageException e)
            {
                return AzureStorageWebServiceUtil.DealWithTheStorageException(e, Request, FileOperationErrorMessage);
            }
        }

        [HttpDelete]
        public HttpResponseMessage DeleteFileInFileShare(string accountName, string sasToken, string fileShareName, string absolutePath)
        {
            try
            {
                AzureFileStorageAdapter fileStorageAdapter = new AzureFileStorageAdapter(accountName, AzureStorageWebServiceUtil.DecodeParamter(sasToken));
                var opResult = fileStorageAdapter.DeleteFile(fileShareName, absolutePath);
                return AzureStorageWebServiceUtil.ConstructHttpResponseUsingOperationResult(Request, opResult);
            }
            catch (StorageException e)
            {
                return AzureStorageWebServiceUtil.DealWithTheStorageException(e, Request, FileOperationErrorMessage);
            }
        }

        //回头要改成Post
        [HttpPost]
        public HttpResponseMessage CreateFileInFileShare(string accountName, string sasToken, string fileShareName, string absolutePath, int size)
        {
            try
            {
                AzureFileStorageAdapter fileStorageAdapter = new AzureFileStorageAdapter(accountName, AzureStorageWebServiceUtil.DecodeParamter(sasToken));

                var opResult = fileStorageAdapter.CreateFile(fileShareName, absolutePath, size);
                return AzureStorageWebServiceUtil.ConstructHttpResponseUsingOperationResult(Request, opResult);
            }
            catch (StorageException e)
            {
                return AzureStorageWebServiceUtil.DealWithTheStorageException(e, Request, FileOperationErrorMessage);
            }
        }
    }
}
