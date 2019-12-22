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
            if (message == null)
            {
                return StatusCode(500);
            }
            return Ok(message);
        }


        [HttpGet("Statistic/{testId}")]
        public ActionResult GetStatistic(int testId)
        {
            try
            {
                return Ok(StatisticDAO.GetStatisticByTestId(testId));
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
            
        }

        [HttpGet("History/{accountId}")]
        public ActionResult GetHistory(int? accountId)
        {
            try
            {
                return Ok(StatisticDAO.GetHistory(accountId));
            }
            catch(Exception e)
            {
                return StatusCode(500);
            }
            
        }
        [HttpPost("Gen/")]
        public ActionResult<IEnumerable<string>> GenTest([FromBody]EmployeeTestDTO employeeTest)
        {
            try
            {
                SystemDAO.GenerateTest(employeeTest.accountIds, employeeTest.configId, employeeTest.oneForAll);

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("GenApplicantTest/")]
        public ActionResult<IEnumerable<string>> GenApplicantTest([FromBody] ApplicantTestDTO applicantTest)
        {
            try
            {
                SystemDAO.GenerateTestForApplicants(applicantTest.configId, applicantTest.applicants);

                return Ok();
            }
            catch(Exception e)
            {
                return StatusCode(500);
            }

        }
    }
}
