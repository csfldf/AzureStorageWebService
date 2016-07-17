using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AzureStorageWebService.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Auth;
using System.Text;

namespace AzureStorageWebService.Controllers
{
    public class TestController : ApiController
    {
        public TestModel PostTM([FromBody] TestModel tm)
        {
            return tm;
        }

        public string GetAll()
        {
            return "aaa";
        }
    }
}
