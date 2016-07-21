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

namespace AzureStorageWebService.Controllers
{
    public class FileServiceController : ApiController
    {
        public IEnumerable<string> GetAllFileShares(string accountName, string sasToken)
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

                return fileSharesNames;
            }
            catch (WebException e)
            {
                throw AzureStorageWebServiceUtil.ConstructHttpResponseException(HttpStatusCode.Unauthorized, "accountName or sasToken error", "---" + e.GetType() + "---" + e.InnerException.GetType());
            }
            catch (StorageException e)
            {
                throw AzureStorageWebServiceUtil.ConstructHttpResponseException(HttpStatusCode.Unauthorized, "accountName or sasToken error", "---" + e.GetType() + "---" + e.InnerException.GetType());
            }
        }

        public IEnumerable<IListFileItem> GetRootDirectoryContentInFileShare(string accountName, string sasToken, string fileShareName)
        {
            try
            {
                AzureFileStorageAdapter fileStorageAdapter = new AzureFileStorageAdapter(accountName, AzureStorageWebServiceUtil.DecodeParamter(sasToken));
                return fileStorageAdapter.ListRootDirectoryInFileShare(fileShareName);
            }
            catch (StorageException)
            {
                throw AzureStorageWebServiceUtil.ConstructHttpResponseException(HttpStatusCode.Unauthorized, "accountName or sasToken error", "accountName or sasToken error");
            }
        }

        public IEnumerable<IListFileItem> GetSpecificDirectoryContentInFileShare(string accountName, string sasToken, string fileShareName, string absolutePath)
        {
            try
            {
                AzureFileStorageAdapter fileStorageAdapter = new AzureFileStorageAdapter(accountName, AzureStorageWebServiceUtil.DecodeParamter(sasToken));
                return fileStorageAdapter.ListFilesAndDirectoriesInSpecificDirectory(fileShareName, absolutePath);
            }
            catch (StorageException)
            {
                throw AzureStorageWebServiceUtil.ConstructHttpResponseException(HttpStatusCode.Unauthorized, "accountName or sasToken error", "accountName or sasToken error");
            }
        }

        [HttpDelete]
        public HttpResponseMessage DeleteFileInFileShare(string accountName, string sasToken, string fileShareName, string absolutePath)
        {
            try
            {
                AzureFileStorageAdapter fileStorageAdapter = new AzureFileStorageAdapter(accountName, AzureStorageWebServiceUtil.DecodeParamter(sasToken));
                fileStorageAdapter.DeleteFile(fileShareName, absolutePath);
                return Request.CreateResponse(HttpStatusCode.OK, new BoolResultOpResponse(true, string.Format("Delete file {0} in file share {1} successfully", absolutePath, fileShareName)));
            }
            catch (StorageException)
            {
                throw AzureStorageWebServiceUtil.ConstructHttpResponseException(HttpStatusCode.Unauthorized, "accountName or sasToken error", "accountName or sasToken error");
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
            catch (StorageException)
            {
                throw AzureStorageWebServiceUtil.ConstructHttpResponseException(HttpStatusCode.Unauthorized, "accountName or sasToken error", "accountName or sasToken error");
            }
        }
    }
}
