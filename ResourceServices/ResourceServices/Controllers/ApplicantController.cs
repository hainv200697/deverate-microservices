using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ResourceServices.Model;
using Newtonsoft.Json;
using ResourceServices.Models;
using AuthenServices.Service;
using ResourceServices.RabbitMQ;

namespace ResourceServices.Controllers
{
    [Route("api/[controller]")]
    public class ApplicantController : Controller
    {
        [HttpPost("CreateApplicant")]
        public ActionResult CreateApplicant([FromBody] List<ApplicantDTO> listApplicant, int configId)
        {
            try
            {
                if(listApplicant == null)
                {
                    return BadRequest();
                }
                foreach(var item in listApplicant)
                {
                    if(item.email == null || item.fullname == null)
                    {
                        return BadRequest();
                    }
                }
                List<ApplicantDTO> applicants = new List<ApplicantDTO>();
                applicants = ApplicantDAO.createApplicant(listApplicant);
                ApplicantTestDTO appTest = new ApplicantTestDTO(configId, applicants);
                Producer producer = new Producer();
                producer.PublishMessage(JsonConvert.SerializeObject(appTest), "GenerateApplicantTest");
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

    }
}
