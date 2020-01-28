using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TestManagementServices.Model;
using TestManagementServices.Models;
using TestManagementServices.Service;

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
            if (con.status == "Pending" && con.accountId != null && DateTime.Compare(DateTime.UtcNow, con.startDate.Value.AddDays(con.expiredDays)) > 0)
            {
                SystemDAO.ExpireTest(testId);
                con = SystemDAO.GetConfig(context, testId);
            }
            else if (con.status == "Doing" && con.timeRemaining <= 10)
            {
                SystemDAO.AutoSubmit(testId);
                con = SystemDAO.GetConfig(context, testId);
            }
            return Ok(con);
        }

        [HttpPost("MyTest")]
        public IActionResult QueryQuestionInMyTest([FromBody]TestInfoDTO testInfo)
        {
            var listQuestion = SystemDAO.GetQuestionInTest(testInfo, true);
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
                int applicantId = SystemDAO.GetApplicantId(userTest.testId);
                return Ok(applicantId);
            }
            catch (Exception)
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
                }
                else
                {
                    return BadRequest("{\"message\" : \"Code invalid, Auto Save\"}");
                }
            }
            catch (Exception)
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

        [HttpGet("GetGeneralStatisticOfApplicant")]
        public IActionResult GetGeneralStatisticOfApplicant(int? accountId)
        {
            try
            {
                return Ok(StatisticDAO.GetGeneralStatisticOfApplicantByTestOwnerId(accountId));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("GetGeneralStatistic")]
        public IActionResult GetGeneralStatistic(int? accountId)
        {
            try
            {
                return Ok(StatisticDAO.GetGeneralStatisticByTestOwnerId(accountId));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
        [HttpGet("GetRankStatisticOfApplicant")]
        public IActionResult GetRankStatisticOfApplicant(int? accountId)
        {
            try
            {
                return Ok(StatisticDAO.GetRankStatisticOfApplicantByTestOwnerId(accountId));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("GetRankStatistic")]
        public IActionResult GetRankStatistic(int? accountId)
        {
            try
            {
                return Ok(StatisticDAO.GetRankStatisticByTestOwnerId(accountId));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("GetOverallPointStatistic")]
        public IActionResult GetOverallPointStatistic(int? companyId, int? configId, bool isEmployee)
        {
            try
            {
                return Ok(StatisticDAO.GetOverallPointStatisticByCompanyId(companyId, configId, isEmployee));

            }
            catch (Exception)
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
            try
            {
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

                return Ok(SystemDAO.CreateSampleTest(sample));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("GetInfoByTestId")]
        public IActionResult GetInfoByTestId(int testId)
        {
            try
            {
                return Ok(StatisticDAO.GetInfoByTestId(testId));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("CheckCode")]
        public IActionResult CheckCode(int testId, string code)
        {
            try
            {
                int check = SystemDAO.CheckCode(testId, code);
                if (check == 0)
                {
                    return BadRequest();
                }
                return Ok(check);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("CatalogueStatisticApplicant")]
        public IActionResult CatalogueStatisticApplicant(int configId, DateTime? from, DateTime? to)
        {
            try
            {
                return Ok(StatisticDAO.CatalogueStatisticApplicant(configId, from, to));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("RankStatisticApplicant")]
        public IActionResult RankStatisticApplicant(int configId, DateTime? from, DateTime? to)
        {
            try
            {
                return Ok(StatisticDAO.RankStatisticApplicant(configId, from, to));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }

}
