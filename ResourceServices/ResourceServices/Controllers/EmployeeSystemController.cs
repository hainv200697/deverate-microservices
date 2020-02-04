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
    public class EmployeeSystemController : Controller
    {
        [HttpPost("SystemCreateEmployee")]
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
                var check = AccountDAO.checkExistedEmail(listemail, companyId);
                if (check.Count > 0)
                {       
                    return BadRequest(check);
                }
                Producer producer = new Producer();
                producer.PublishMessage(JsonConvert.SerializeObject(ListAccountGenerate), "ListAccountGenerate");
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("SystemResendPassword")]
        public ActionResult ResendPassword([FromBody] List<string> ListAccountSendPass, int companyId)
        {
            try
            {
                var check = AccountDAO.checkExistedAccount(ListAccountSendPass, companyId);
                if (check.Count < ListAccountSendPass.Count)
                {
                    return BadRequest(check);
                }
                Producer producer = new Producer();
                producer.PublishMessage(JsonConvert.SerializeObject(ListAccountSendPass), "ResendPassword");
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("SystemUpdateEmployeeStatus")]
        public ActionResult UpdateEmployeeStatus([FromBody] List<int> listEmpId,bool status)
        {
            try
            {
                if (listEmpId == null)
                {
                    return BadRequest();
                }
                AccountDAO.UpdateEmployeeStatus(listEmpId,status);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("SystemGetAccountByRole")]
        public ActionResult GetAccountByRole(int companyId, int role)
        {
            try
            {
                if (companyId == 0)
                {
                    return BadRequest();
                }
                List<AccountDTO> listAccount = AccountDAO.GetAccountByRole(companyId, role);
                return Ok(listAccount);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

    }
}
