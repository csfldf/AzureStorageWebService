using System;
using System.Collections.Generic;
using AzureStorageWebService.Exceptions;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.File;
using AzureStorageWebService.ResponseMessage.Model.File;

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

        public IEnumerable<FileorDirectoryModel> ListRootDirectoryInFileShare(string fileShareName)
        {
            CloudFileShare cloudFileShare = cloudFileClient.GetShareReference(fileShareName);
            CloudFileDirectory root = cloudFileShare.GetRootDirectoryReference();
            return TransFormIListFileItem(root.ListFilesAndDirectories());
        }

        public IEnumerable<FileorDirectoryModel> ListFilesAndDirectoriesInSpecificDirectory(string fileShareName, string absolutePath)
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
            return TransFormIListFileItem(currentDirectory.ListFilesAndDirectories());
        }

        public Tuple<bool, string> DeleteFile(string fileShareName, string absolutePath)
        {
            CloudFileShare cloudFileShare = cloudFileClient.GetShareReference(fileShareName);
            CloudFileDirectory currentDirectory = cloudFileShare.GetRootDirectoryReference();

            string[] pathParts = absolutePath.Substring(1).Split(AzureStorageWebServiceConstantValue.pathSplitor);

            if (pathParts.Length == 0)
            {
                return Tuple.Create<bool, string>(false, string.Format("path = {0} is illegal", absolutePath));
            }

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

            if (pathParts.Length == 0)
            {
                return Tuple.Create<bool, string>(false, string.Format("path = {0} is illegal in file share {1}", absolutePath, fileShareName));
            }

            CloudFile targetFile = null;
            for (int i = 0; i < pathParts.Length; i++)
            {
                if (i == pathParts.Length - 1)
                {
                    targetFile = currentDirectory.GetFileReference(pathParts[i]);

                    if (targetFile.Exists())
                    {
                        return Tuple.Create<bool, string>(false, string.Format("file with path = {0} in file share {1} exists", absolutePath, fileShareName));
                    }

                    targetFile.Create(size);
                }
                else
                {
                    currentDirectory = currentDirectory.GetDirectoryReference(pathParts[i]);

                    if (!currentDirectory.Exists())
                    {
                        return Tuple.Create<bool, string>(false, string.Format("path = {0} is illegal in file share {1}", absolutePath, fileShareName));
                    }
                }
            }
            return Tuple.Create<bool, string>(true, string.Format("Create file {0} in file share {1} successfully", absolutePath, fileShareName));
        }

        public Tuple<bool, string> CreateDirectory(string fileShareName, string absolutePath)
        {
            CloudFileShare cloudFileShare = cloudFileClient.GetShareReference(fileShareName);
            CloudFileDirectory currentDirectory = cloudFileShare.GetRootDirectoryReference();

            string[] pathParts = absolutePath.Substring(1).Split(AzureStorageWebServiceConstantValue.pathSplitor);

            if (pathParts.Length == 0)
            {
                return Tuple.Create<bool, string>(false, string.Format("path = {0} is illegal in file share {1}", absolutePath, fileShareName));
            }

            CloudFileDirectory targetDirectory = null;
            for (int i = 0; i < pathParts.Length; i++)
            {
                if (i == pathParts.Length - 1)
                {
                    targetDirectory = currentDirectory.GetDirectoryReference(pathParts[i]);

                    if (targetDirectory.Exists())
                    {
                        return Tuple.Create<bool, string>(false, string.Format("Directory with path = {0} in file share {1} exists", absolutePath, fileShareName));
                    }

                    targetDirectory.Create();
                }
                else
                {
                    currentDirectory = currentDirectory.GetDirectoryReference(pathParts[i]);

                    if (!currentDirectory.Exists())
                    {
                        return Tuple.Create<bool, string>(false, string.Format("path = {0} is illegal in file share {1}", absolutePath, fileShareName));
                    }
                }
            }
            return Tuple.Create<bool, string>(true, string.Format("Create directory {0} in file share {1} successfully", absolutePath, fileShareName));
        }

        private IEnumerable<FileorDirectoryModel> TransFormIListFileItem(IEnumerable<IListFileItem> items)
        {
            List<FileorDirectoryModel> fileOrDirectories = new List<FileorDirectoryModel>();

            foreach (IListFileItem item in items)
            {
                if (item is CloudFile)
                {
                    CloudFile targetFile = (CloudFile)item;

                    if (targetFile.Properties.LastModified == null)
                    {
                        fileOrDirectories.Add(new FileorDirectoryModel(targetFile.Name, null, true));
                    }
                    else
                    {
                        fileOrDirectories.Add(new FileorDirectoryModel(targetFile.Name, targetFile.Properties.LastModified.Value.DateTime, true));
                    }
                }
                else if (item is CloudFileDirectory)
                {
                    CloudFileDirectory targetDirectory = (CloudFileDirectory)item;

                    if (targetDirectory.Properties.LastModified == null)
                    {
                        fileOrDirectories.Add(new FileorDirectoryModel(targetDirectory.Name, null, true));
                    }
                    else
                    {
                        fileOrDirectories.Add(new FileorDirectoryModel(targetDirectory.Name, targetDirectory.Properties.LastModified.Value.DateTime, true));
                    }
                }
            }

            return fileOrDirectories;
        }
    }
}