﻿using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.File;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AzureStorageWebService.Exceptions;

namespace AzureStorageWebService.Utils
{
    public class AzureFileStorageAdapter
    {
        private CloudFileClient cloudFileClient;

        public AzureFileStorageAdapter(string accountName, string sasToken)
        {
            StorageCredentials accountSAS = new StorageCredentials(sasToken);
            CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(accountSAS, blobEndpoint: null, queueEndpoint: null, tableEndpoint: null, fileEndpoint: new Uri("https://" + accountName + ".file.core.windows.net"));
            cloudFileClient = cloudStorageAccount.CreateCloudFileClient();
        }

        public IEnumerable<CloudFileShare> GetAllFileShares()
        {
            return cloudFileClient.ListShares();
        }

        public IEnumerable<IListFileItem> ListRootDirectoryInFileShare(string fileShareName)
        {
            CloudFileShare cloudFileShare = cloudFileClient.GetShareReference(fileShareName);
            CloudFileDirectory root = cloudFileShare.GetRootDirectoryReference();
            return root.ListFilesAndDirectories();
        }

        public IEnumerable<IListFileItem> ListFilesAndDirectoriesInSpecificDirectory(string fileShareName, string absolutePath)
        {
            CloudFileShare cloudFileShare = cloudFileClient.GetShareReference(fileShareName);
            CloudFileDirectory currentDirectory = cloudFileShare.GetRootDirectoryReference();

            string[] pathParts = absolutePath.Substring(1).Split(AzureStorageWebServiceConstantValue.pathSplitor);

            foreach (string part in pathParts)
            {
                currentDirectory = currentDirectory.GetDirectoryReference(part);

                if (!currentDirectory.Exists())
                {
                    throw new OperationFailException(string.Format("No directory with path = {0}", absolutePath));
                }
            }
            return currentDirectory.ListFilesAndDirectories();
        }

        public Tuple<bool, string> DeleteFile(string fileShareName, string absolutePath)
        {
            CloudFileShare cloudFileShare = cloudFileClient.GetShareReference(fileShareName);
            CloudFileDirectory currentDirectory = cloudFileShare.GetRootDirectoryReference();

            string[] pathParts = absolutePath.Substring(1).Split(AzureStorageWebServiceConstantValue.pathSplitor);
            CloudFile targetFile = null;
            for (int i = 0; i < pathParts.Length; i++)
            {
                if (i == pathParts.Length - 1)
                {
                    targetFile = currentDirectory.GetFileReference(pathParts[i]);

                    if (!targetFile.Exists())
                    {
                        return Tuple.Create<bool, string>(false, string.Format("No file with path = {0} in file share {1}", absolutePath, fileShareName));
                    }
                }
                else 
                {
                    currentDirectory = currentDirectory.GetDirectoryReference(pathParts[i]);

                    if (!currentDirectory.Exists())
                    {
                        return Tuple.Create<bool, string>(false, string.Format("No file with path = {0} in file share {1}", absolutePath, fileShareName));
                    }
                }
            }

            targetFile.Delete();
            return Tuple.Create<bool, string>(true, string.Format("Delete file {0} in file share {1} successfully", absolutePath, fileShareName));
        }

        public Tuple<bool, string> CreateFile(string fileShareName, string absolutePath, int size)
        {
            CloudFileShare cloudFileShare = cloudFileClient.GetShareReference(fileShareName);
            CloudFileDirectory currentDirectory = cloudFileShare.GetRootDirectoryReference();

            string[] pathParts = absolutePath.Substring(1).Split(AzureStorageWebServiceConstantValue.pathSplitor);
            CloudFile targetFile = null;
            for (int i = 0; i < pathParts.Length; i++)
            {
                if (i == pathParts.Length - 1)
                {
                    targetFile = currentDirectory.GetFileReference(pathParts[i]);

                    if (!targetFile.Exists())
                    {
                        return Tuple.Create<bool, string>(false, string.Format("No file with path = {0} in file share {1}", absolutePath, fileShareName));
                    }

                    targetFile.Create(size);
                }
                else
                {
                    currentDirectory = currentDirectory.GetDirectoryReference(pathParts[i]);

                    if (!currentDirectory.Exists())
                    {
                        return Tuple.Create<bool, string>(false, string.Format("No file with path = {0} in file share {1}", absolutePath, fileShareName));
                    }
                }
            }
            return Tuple.Create<bool, string>(true, string.Format("Create file {0} in file share {1} successfully", absolutePath, fileShareName));
        }
    }
}