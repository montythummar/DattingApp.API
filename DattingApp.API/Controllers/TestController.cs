using System.Collections.Generic;
using System.Threading.Tasks;
using DattingAppDataLayer;
using DattingAppDataTransferObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DattingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {        
        [HttpGet]
        public async Task<IActionResult> GetTestData()
        {
            TestDAL objDbAccess = new TestDAL();
            List<TestDto> lstTest = new List<TestDto>();
            lstTest = await objDbAccess.GetTestData();
            return Ok(lstTest);
        }
    }
}