using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureStorageWebService.ParamsModel.Queue
{
    public class QueueOperationParamsModel :BaseParamsModel
    {
        public string QueueName
        {
            get;
            set;
        }

        public string MessageCount
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }
    }
}