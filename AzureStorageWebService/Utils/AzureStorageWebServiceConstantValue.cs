using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureStorageWebService.Utils
{
    public static class AzureStorageWebServiceConstantValue
    {
        public const int SleepTime = 200;
        public const char pathSplitor = '/';
        public const string UnAuthorizedMessage = "Unauthorized operation, accountName or sasToken may be error.";
    }
}