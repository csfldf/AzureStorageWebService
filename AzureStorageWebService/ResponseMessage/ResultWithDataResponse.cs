using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureStorageWebService.ResponseMessage
{
    public class ResultWithDataResponse<T>
    {
        public bool IsSuccessful
        {
            get;
            set;
        }

        public T Data
        {
            get;
            set;
        }

        public ResultWithDataResponse(bool isSuccessful, T data)
        {
            IsSuccessful = isSuccessful;
            Data = data;
        }
    }
}