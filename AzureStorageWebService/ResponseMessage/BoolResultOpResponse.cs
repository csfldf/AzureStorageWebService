using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureStorageWebService.ResponseMessage
{
    public class BoolResultOpResponse
    {
        public bool IsSuccessful
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public BoolResultOpResponse(bool isSuccessful, string message)
        {
            IsSuccessful = isSuccessful;
            Message = message;
        }
    }
}