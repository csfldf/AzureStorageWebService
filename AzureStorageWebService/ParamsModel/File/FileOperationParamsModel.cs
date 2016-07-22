using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureStorageWebService.ParamsModel.File
{
    public class FileOperationParamsModel :BaseParamsModel
    {
        public string FileShareName
        {
            get;
            set;
        }

        public string AbsolutePath
        {
            get;
            set;
        }

        public int Size
        {
            get;
            set;
        }
    }
}