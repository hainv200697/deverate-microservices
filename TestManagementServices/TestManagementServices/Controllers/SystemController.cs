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


        [HttpGet("Statistic/{testId}")]
        public ActionResult<IEnumerable<string>> GetStatistic(int? testId)
        {

            
            return new JsonResult(rm.Success(Message.createSucceed, StatisticDAO.GetStatisticByAccountId(testId)));
        }

        [HttpGet("History/{accountId}")]
        public ActionResult<IEnumerable<string>> GetHistory(int? accountId)
        {


            return new JsonResult(rm.Success(Message.createSucceed, StatisticDAO.GetHistory(accountId)));
        }
        [HttpGet("Gen/")]
        public ActionResult<IEnumerable<string>> GenTest(string configId)
        {
            SystemDAO.GenerateTest(configId);

            return new JsonResult(rm.Success(Message.createSucceed));
        }
    }
}
