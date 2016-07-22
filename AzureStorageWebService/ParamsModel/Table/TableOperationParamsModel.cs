using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureStorageWebService.ParamsModel.Table
{
    public class TableOperationParamsModel :BaseParamsModel
    {
        public string TableName
        {
            get;
            set;
        }
    }
}