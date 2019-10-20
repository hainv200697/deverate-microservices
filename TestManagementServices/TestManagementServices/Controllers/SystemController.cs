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

        [HttpGet("SendTestMail/{configId}")]
        public ActionResult<IEnumerable<string>> SendTestMail(int? configId)
        {

            string message = SystemDAO.SendTestMail(configId, false);
            if(message == null)
            {
                return new JsonResult(rm.Error(Message.noEmployeeException));
            }
            return new JsonResult(rm.Success(message));
        }

        [HttpPost("EvaluateRank")]
        public ActionResult<IEnumerable<string>> PostEvaluateRank([FromBody]TestAnswerDTO answer)
        {

            RankPoint rp = SystemDAO.EvaluateRank(context, answer);
            if (rp == null)
            {
                return new JsonResult(rm.Error(Message.evaluateFailed));
            }
            return new JsonResult(rm.Success(Message.evaluateSucceed, rp));
        }

        [HttpGet("Statistic/{accountId}")]
        public ActionResult<IEnumerable<string>> GetStatistic(int? accountId)
        {

            
            return new JsonResult(rm.Success(Message.createSucceed, StatisticDAO.GetStatisticByAccountId(accountId)));
        }
    }
}
