using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AuthenServices.Models;
using Newtonsoft.Json;
using AuthenServices.Service;
using AuthenServices.RabbitMQ;

namespace ResourceServices.Controllers
{
    [Route("api/[controller]")]
    public class EmployeeController : Controller
    {
        [HttpPost("CreateEmployee")]
        public ActionResult CreateEmployee([FromBody] List<MessageAccount> ListAccountGenerate)
        {
            try
            {
                List<string> listemail = new List<string>();
                foreach (var emp in ListAccountGenerate)
                {

                    listemail.Add(emp.Email);
                }
                var existedMail = listemail.GroupBy(email => email).Where(g => g.Count() > 1).Select(g => g.Key);
                if (existedMail.Count() > 0)
                {
                    return BadRequest(existedMail);
                }
                int? companyId = ListAccountGenerate[0].CompanyId;
                var check = AccountDAO.CheckExistedEmail(listemail, companyId);
                if (check.Count > 0)
                {
                    return BadRequest(check);
                }
                Producer producer = new Producer();
                List<MessageAccountDTO> msAccount = AccountDAO.GenerateCompanyAccount(ListAccountGenerate);
                if (msAccount != null)
                {
                    foreach(MessageAccountDTO item in msAccount)
                    {
                        producer.PublishMessage(JsonConvert.SerializeObject(item), "AccountToEmail");
                    }
                }
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}