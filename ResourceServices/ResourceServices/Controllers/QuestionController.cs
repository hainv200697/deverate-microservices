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
                if (question == null)
                {
                    return BadRequest();
                }
                var message = QuestionDAO.CreateQuestion(question);
                return Ok(message);
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
                var message = QuestionDAO.UpdateQuestion(ques);
                return Ok(message);
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
                var message = QuestionDAO.removeQuestion(ques);
                return Ok(message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

    }
}
