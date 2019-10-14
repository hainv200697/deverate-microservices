using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenServices.Models;
using Microsoft.AspNetCore.Mvc;
using TestManagementServices.Model;
using TestManagementServices.Models;
using TestManagementServices.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestManagementServices.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        ResponseMessage rm = new ResponseMessage();
        DeverateContext context;

        public TestController(DeverateContext context)
        {
            this.context = context;
        }
        [HttpGet("MyConfigTest/{accountId}")]
        public ActionResult<IEnumerable<string>> GetAllConfigTestInfoToday(int accountId)
        {
            List<ConfigurationDTO> com = SystemDAO.GetAllConfigTestTodayByUsername(context, accountId);
            return new JsonResult(rm.Success(com));
        }

        [HttpPost("MyTest")]
        public IActionResult QueryQuestionInMyTest([FromBody]QueryTest queryTest) 
        {
            var listQuestion = SystemDAO.GetQuestionInTest(context, queryTest);
            if (listQuestion == null)
            {
                return BadRequest("Code invalid");
            }
            return Ok(listQuestion);
        }
    }
}
