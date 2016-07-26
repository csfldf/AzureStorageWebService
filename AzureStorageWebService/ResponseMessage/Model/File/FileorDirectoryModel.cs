using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureStorageWebService.ResponseMessage.Model.File
{
    public class FileorDirectoryModel
    {
        public string Name
        {
            get;
            set;
        }

        public DateTime LastModifiedTime
        {
            get;
            set;
        }

        public bool IsDirectory
        {
            get;
            set;
        }

        public FileorDirectoryModel(string name, DateTime lastModifiedTime, bool isDirectory)
        {
            Name = name;
            LastModifiedTime = lastModifiedTime;
            IsDirectory = isDirectory;
        }
    }
}