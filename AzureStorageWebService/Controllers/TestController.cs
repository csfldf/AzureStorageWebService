﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AzureStorageWebService.Models;

namespace AzureStorageWebService.Controllers
{
    public class TestController : ApiController
    {
        public TestModel PostTM([FromBody] TestModel tm)
        {
            return tm;
        }
    }
}
