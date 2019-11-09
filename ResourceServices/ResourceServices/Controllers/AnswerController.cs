using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AuthenServices.Models;
using System.Net.Http;
using ResourceServices.Model;
using ResourceServices.Service;
using System.Net;
using Newtonsoft.Json;
using ResourceServices.Models;
using Microsoft.AspNetCore.Authorization;

namespace ResourceServices.Controllers
{
    [Route("api/[controller]")]
    public class AnswerController : Controller
    {
        

        [HttpGet("GetAnswerByQuestion")]
        public ActionResult GetAnswerByCatalogueId(int id ,bool status)
        {
            try
            {
                List<AnswerDTO> Answers = AnswerDAO.GetAnswerByQuestion(id, status);
                return Ok(Answers);
            }
            catch (Exception )
            {
                return StatusCode(500);
            }
        }

        

        

        [HttpPost("CreateAnswer")]
        public ActionResult CreateAnswer([FromBody] AnswerDTO answer)
        {
            try
            {
                List<AnswerDTO> Answers = AnswerDAO.GetAnswerByQuestion(answer.questionId, true);
                if (Answers.Count() > 6)
                {
                    return BadRequest();
                }
                var message = AnswerDAO.CreateAnswer(answer);
                AnswerDAO.UpdateMaxPoint(answer.questionId);
                return Ok(message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }


        [HttpPut("UpdateAnswer")]
        public ActionResult UpdateAnswer([FromBody]AnswerDTO ans)
        {
            try
            {
                var message = AnswerDAO.UpdateAnswer(ans);
                AnswerDAO.UpdateMaxPoint(ans.questionId);
                return Ok(message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut]
        [Route("RemoveAnswer")]
        public ActionResult RemoveAnswer([FromBody] List<AnswerDTO> ans)
        {
            try
            {
                int? id = ans[0].questionId;
                var message = AnswerDAO.removeAnswer(ans);
                AnswerDAO.UpdateMaxPoint(id);
                return Ok(message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

    }
}
