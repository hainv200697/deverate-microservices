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
    public class EmployeeController : Controller
    {

        ResponseMessage rm = new ResponseMessage();
        DeverateContext context;

        public EmployeeController(DeverateContext context)
        {
            this.context = context;
        }

        [HttpGet("GetEmployee")]
        public ActionResult EmployeeByCompany(int? companyId, bool? status)
        {
            try
            {
                if (companyId == null)
                {
                    return BadRequest();
                }
                List<AccountDTO> listEmployee = AccountDAO.GetEmployee(companyId, status);

                return Ok(rm.Success(listEmployee));
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

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
                int? companyId = ListAccountGenerate[0].CompanyId;
                var check = context.Account.Where(x => listemail.Contains(x.Email) && x.CompanyId == companyId).Select(x=> x.Email ).ToList();
                if (check.Count > 0)
                {
                    return BadRequest("Email "+ check[0] +" is Existed");
                }
                Producer producer = new Producer();
                producer.PublishMessage(JsonConvert.SerializeObject(ListAccountGenerate), "AccountGenerate");
                return Ok(rm.Success("Create success"));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("ResendPassword")]
        public ActionResult ResendPassword([FromBody] List<AccountDTO> ListAccountSendPass)
        {
            try
            {
                List<string> listUsername = new List<string>();
                foreach (var emp in ListAccountSendPass)
                {

                    listUsername.Add(emp.username);
                }
                int? companyId = ListAccountSendPass[0].companyId;
                var check = context.Account.Where(x => listUsername.Contains(x.Username) && x.CompanyId == companyId).Select(x => x.Username).ToList();
                if (check.Count < listUsername.Count)
                {
                    return BadRequest("Account " + check[0] + " is not Existed");
                }
                Producer producer = new Producer();
                producer.PublishMessage(JsonConvert.SerializeObject(listUsername), "AccountGenerate");
                return Ok(rm.Success("Password was send to your email!"));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut("RemoveEmployee")]
        public ActionResult RemoveEmployee([FromBody] List<AccountDTO> employee)
        {
            try
            {
                if (employee == null)
                {
                    return BadRequest();
                }
                var listEmployee = AccountDAO.UpdateEmployeeStatus(employee);

                return Ok(rm.Success("Status is changed"));
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }
    }
}
