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
using ResourceServices.Models;

namespace ResourceServices.Controllers
{
    [Route("api/[controller]")]
    public class QuestionController : Controller
    {
        ResponseMessage _rm = new ResponseMessage();
        DeverateContext context;

        [HttpGet]
        [Route("GetAllQuestion")]
        public ActionResult GetAllCatelogue()
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

        [HttpPost]
        [Route("CreateQuestion")]
        public ActionResult CreateQuestion(QuestionDTO ques)
        {
            try
            {
                string message = QuestionDAO.CreateQuestion(ques);
                return Ok(message);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpPost]
        [Route("UpdateQuestion")]
        public ActionResult UpdateQuestion(QuestionDTO ques)
        {
            try
            {
                string message = QuestionDAO.UpdateQuestion(ques);
                return Ok(message);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("RemoveQuestion")]
        public ActionResult RemoveQuestion(List<QuestionDTO> ques)
        {
            try
            {
                string message = QuestionDAO.removeQuestion(ques);
                return Ok(message);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
