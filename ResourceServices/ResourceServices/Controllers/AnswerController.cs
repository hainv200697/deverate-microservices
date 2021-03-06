﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ResourceServices.Model;
using ResourceServices.Service;

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
                AnswerDAO.CreateAnswer(answer);
                return Ok(Message.createAnswerSucceed);
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
                AnswerDAO.UpdateAnswer(ans);
                return Ok(Message.updateAnswerSucceed);
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
                AnswerDAO.removeAnswer(ans);
                return Ok(Message.removeAnswerSucceed);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("GetDefaultAnswerByQuestion")]
        public ActionResult GetDefaultAnswerByCatalogueId(int id, bool status)
        {
            try
            {
                List<AnswerDTO> Answers = AnswerDAO.GetAnswerByQuestion(id, status);
                return Ok(Answers);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }





        [HttpPost("CreateAnswerDefault")]
        public ActionResult CreateAnswerDefault([FromBody] AnswerDTO answer)
        {
            try
            {
                List<AnswerDTO> Answers = AnswerDAO.GetAnswerByQuestion(answer.questionId, true);
                if (Answers.Count() > 6)
                {
                    return BadRequest();
                }
                AnswerDAO.CreateAnswer(answer);
                return Ok(Message.createAnswerSucceed);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }


        [HttpPut("UpdateAnswerDefault")]
        public ActionResult UpdateAnswerDefault([FromBody]AnswerDTO ans)
        {
            try
            {
                AnswerDAO.UpdateAnswer(ans);
                return Ok(Message.updateAnswerSucceed);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut]
        [Route("RemoveAnswerDefault")]
        public ActionResult RemoveAnswerDefault([FromBody] List<AnswerDTO> ans)
        {
            try
            {
                int? id = ans[0].questionId;
                AnswerDAO.removeAnswer(ans);
                return Ok(Message.removeAnswerSucceed);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
