using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureStorageWebService.ParamsModel
{
    public class BaseParamsModel
    {
        public string AccountName
        {
            get;
            set;
        }

        public string SasToken
        {
            get;
            set;
        }
    }
}