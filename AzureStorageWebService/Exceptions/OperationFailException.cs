using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureStorageWebService.Exceptions
{
    public class OperationFailException : ApplicationException
    {
        public OperationFailException(string msg)
            : base(msg)
        {

        }
    }
}