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
using AuthenServices.Model;
using AuthenServices.Service;
using ResourceServices.RabbitMQ;

namespace ResourceServices.Controllers
{
    [Route("api/[controller]")]
    public class ApplicantController : Controller
    {

        ResponseMessage rm = new ResponseMessage();
        DeverateContext context;

        public ApplicantController(DeverateContext context)
        {
            this.context = context;
        }

        [HttpPost("CreateApplicant")]
        public ActionResult CreateApplicant([FromBody] List<ApplicantDTO> listApplicant)
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

                var configId = ApplicantDAO.getConfigApplicant();
                if(configId == null)
                {
                    return NotFound();
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
