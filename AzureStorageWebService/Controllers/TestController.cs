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

        public string GetAll()
        {
            return "aaa";
        }
    }
}
