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
    public class QuestionController : Controller
    {

        [HttpGet("GetQuestionByCatalogue")]
        public ActionResult GetQuestionByCatalogueId(int catalogueId, int companyId,bool status)
        {
            try
            {
             QuestionDTO ques = QuestionDAO.GetQuestionByCatalogue(catalogueId, companyId, status);
                if(ques == null)
                {
                    return BadRequest();
                }
                return Ok(ques);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }


        [HttpPost("CreateQuestion")]
        public ActionResult CreateQuestion([FromBody] List<QuestionDTO> question)
        {
            try
            {
                int companyCatalogue = question[0].companyCatalogueId;
                List<string> listeQues = new List<string>();
                foreach (var ques in question)
                {

                    listeQues.Add(ques.question1);
                }
                var check = QuestionDAO.checkExistedQuestion(listeQues, companyCatalogue);
                if (check.Count() > 0)
                {
                    return BadRequest(check);
                }
                if (question == null)
                {
                    return BadRequest();
                }
                foreach(var item in question)
                {
                    if(item.answer.Count() < 3 || item.answer.Count() > 6)
                    {
                        return BadRequest();
                    }
                }
                QuestionDAO.CreateQuestion(question);
                return Ok(Message.createQuestionSucceed);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }



        [HttpPut("UpdateQuestion")]
        public ActionResult UpdateQuestion([FromBody]QuestionDTO ques)
        {
            try
            {
                if (ques == null)
                {
                    return BadRequest();
                }
                QuestionDAO.UpdateQuestion(ques);
                return Ok(Message.updateQuestionSucceed);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("RemoveQuestion")]
        public ActionResult RemoveQuestion([FromBody] List<QuestionDTO> ques)
        {
            try
            {
                QuestionDAO.removeQuestion(ques);
                return Ok(Message.removeQuestionSucceed);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

    }
}
