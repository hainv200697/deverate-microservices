using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenServices.Models;
using Microsoft.AspNetCore.Http;
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
        DeverateContext context;

        public TestController(DeverateContext context)
        {
            this.context = context;
        }
        [HttpGet("AllMyTestToday/{accountId}")]
        public IActionResult GetAllTestInfoToday(int accountId)
        {
            List<TestInfoDTO> con = SystemDAO.GetAllTestTodayByUsername(context, accountId);
            return Ok(con);
        }

        [HttpGet("GetConfig/{testId}")]
        public IActionResult GetConfig(int testId)
        {
            ConfigurationDTO con = SystemDAO.GetConfig(context, testId);
            if (con.status == "Doing" && con.timeRemaining <= 10)
            {
                SystemDAO.AutoSubmit(testId);
                con = SystemDAO.GetConfig(context, testId);
            }
            return Ok(con);
        }

        [HttpPost("MyTest")]
        public IActionResult QueryQuestionInMyTest([FromBody]TestInfoDTO testInfo) 
        {
            var listQuestion = SystemDAO.GetQuestionInTest(testInfo,true);
            if (listQuestion == null)
            {
                return BadRequest("Code invalid");
            }
            return Ok(listQuestion);
        }

        [HttpPost("SubmitTest")]
        public IActionResult SubmitTest([FromBody] UserTest userTest)
        {
            try
            {
                SystemDAO.SaveAnswer(userTest);
                RankPoint rp = SystemDAO.EvaluateRank(userTest);
                if (rp == null)
                {
                    return BadRequest("Code invalid, Submit fail");
                }
                return Ok(rp);
            } catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("AutoSave")]
        public IActionResult AutoSave([FromBody] UserTest userTest)
        {
            try
            {
                bool save = SystemDAO.AutoSaveAnswer(context, userTest);
                if (save)
                {
                   return Ok("{\"message\" : \"Auto Save Success\"}");
                } else
                {
                    return BadRequest("{\"message\" : \"Code invalid, Auto Save\"}");
                }
            } catch(Exception)
            {
                return StatusCode(500);
            }
            
        }
        [HttpGet("GetAllTest")]
        public IActionResult GetTest(int id)
        {
            List<TestInfoDTO> listTest = SystemDAO.GetTestByConfig(context, id);

            return Ok(listTest);
        }

        [HttpGet("GetGeneralStatistic")]
        public IActionResult GetGeneralStatistic(int? accountId)
        {
            

            return Ok(StatisticDAO.GetGeneralStatisticByTestOwnerId(accountId) );
        }

        [HttpGet("GetRankStatistic")]
        public IActionResult GetRankStatistic(int? accountId)
        {
            return Ok(StatisticDAO.GetRankStatisticByTestOwnerId(accountId));
        }

        [HttpGet("GetOverallPointStatistic")]
        public IActionResult GetOverallPointStatistic(int? companyId, int? configId, bool isEmployee)
        {
            try
            {
                return Ok(StatisticDAO.GetOverallPointStatisticByCompanyId(companyId, configId, isEmployee));
            }
            catch(Exception e)
            {
                return StatusCode(500);
            }

            
        }


        [HttpPost("ManagerInTest")]
        public IActionResult GetQuesionInTest([FromBody]TestInfoDTO testInfo)
        {
            var listQuestion = SystemDAO.GetQuestionInTest(testInfo, false);
            if (listQuestion == null)
            {
                return BadRequest("Code invalid");
            }
            return Ok(listQuestion);
        }

        [HttpPost("SendTestCode")]
        public IActionResult SendTestCode([FromBody]List<int> listestResendCode)
        {
            try { 
                SystemDAO.SendMailQuizCode(listestResendCode, true);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }


        [HttpPost("GenerateSampleTest")]
        public IActionResult SendTestCode([FromBody]SampleConfigDTO sample)
        {
            try
            {
                
                return Ok(SystemDAO.GenerateQuestionsForSampleTest(context, sample));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
    
}
