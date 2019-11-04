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
        [HttpGet]
        [Route("GetAllQuestion")]
        public ActionResult GetAllQuestion()
        {
            try
            {
                List<QuestionDTO> Questions = QuestionDAO.GetAllQuestion();
                return Ok(Questions);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("GetQuestionByCatalogue")]
        public ActionResult GetQuestionByCatalogueId(int catalogueId, int companyId,bool status)
        {
            try
            {
                List<QuestionDTO> Questions = QuestionDAO.GetQuestionByCatalogue(catalogueId, companyId, status);
                if(Questions == null)
                {
                    return BadRequest();
                }
                return Ok(Questions);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        [HttpGet]
        [Route("GetQuestionByStatus")]
        public ActionResult GetQuestionByStatus(bool status,int id)
        {
            try
            {
                List<QuestionDTO> Questions = QuestionDAO.GetQuestionByStatus(status,id);
                return Ok(Questions);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("CreateQuestionExcel")]
        public ActionResult CreateQuestionExcel([FromBody] List<QuestionDTO> question)
        {
            try
                    {
                var message = QuestionDAO.CreateQuestionExcel(question);
                return Ok(message);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("CreateQuestion")]
        public ActionResult CreateQuestion([FromBody] QuestionDTO question)
        {
            try
            {
                //List<QuestionDTO> question = JsonConvert.DeserializeObject<QuestionDTO>(quest);
                var message = QuestionDAO.CreateQuestion(question);
                ////string message = "avc";
                return Ok(message);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpPut]
        [Route("UpdateQuestion")]
        public ActionResult UpdateQuestion([FromBody]QuestionDTO ques)
        {
            try
            {
                var message = QuestionDAO.UpdateQuestion(ques);
                return Ok(message);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("RemoveQuestion")]
        public ActionResult RemoveQuestion([FromBody] List<QuestionDTO> ques)
        {
            try
            {
                var message = QuestionDAO.removeQuestion(ques);
                return Ok(message);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

    }
}
