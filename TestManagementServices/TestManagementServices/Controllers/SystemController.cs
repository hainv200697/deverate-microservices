using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenServices.Models;
using Microsoft.AspNetCore.Mvc;
using TestManagementServices.Model;
using TestManagementServices.Models;
using TestManagementServices.Service;

namespace TestManagementServices.Controllers
{
    [Route("api/[controller]")]
    public class SystemController : Controller
    {
        ResponseMessage rm = new ResponseMessage();
        DeverateContext context;

        public SystemController(DeverateContext context)
        {
            this.context = context;
        }

        [HttpPost("EvaluateRank")]
        public ActionResult<IEnumerable<string>> PostEvaluateRank([FromBody]TestAnswerDTO answer)
        {

            RankPoint rp = SystemDAO.EvaluateRank(context, answer);
            if(rp == null)
            {
                return new JsonResult(rm.Error(Message.evaluateFailed));
            }
            return new JsonResult(rm.Success(Message.evaluateSucceed, rp));
        }

        [HttpPost("GenTest")]
        public ActionResult<IEnumerable<string>> GenTest([FromBody]ConfigurationDTO configuration)
        {

            string message = SystemDAO.GenerateTest(context, configuration);
            if(message != null)
            {
                return new JsonResult(rm.Error(message));
            }
            return new JsonResult(rm.Success(Message.createSucceed));
        }
    }
}
