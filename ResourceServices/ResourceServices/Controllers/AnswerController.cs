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
        

        [HttpGet]
        [Route("GetAnswerByQuestion")]
        public ActionResult GetAnswerByCatalogueId(int id)
        {
            try
            {
                List<AnswerDTO> Answers = AnswerDAO.GetAnswerByQuestion(id);
                return Ok(Answers);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        

        

        [HttpPost]
        [Route("CreateAnswer")]
        public ActionResult CreateAnswer([FromBody] AnswerDTO Answer)
        {
            try
            {
                var message = AnswerDAO.CreateAnswer(Answer);
                return Ok(message);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpPut]
        [Route("UpdateAnswer")]
        public ActionResult UpdateAnswer([FromBody]AnswerDTO ans)
        {
            try
            {
                var message = AnswerDAO.UpdateAnswer(ans);
                return Ok(message);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("RemoveAnswer")]
        public ActionResult RemoveAnswer([FromBody] List<AnswerDTO> ans)
        {
            try
            {
                var message = AnswerDAO.removeAnswer(ans);
                return Ok(message);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

    }
}
