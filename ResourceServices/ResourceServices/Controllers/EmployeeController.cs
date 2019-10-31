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

        [HttpGet]
        [Route("GetEmployee")]
        public ActionResult GetEmployeeByCompany(int? id)
        {
            try
            {
                int? companyId = AccountDAO.GetCompany(id);
                List<AccountDTO> listEmployee = AccountDAO.GetEmployee(companyId);
                return Ok(rm.Success(listEmployee));
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("CreateEmployee")]
        public ActionResult CreateEmployee([FromBody] AccountDTO employee)
        {
            try
            {
                int? companyId = AccountDAO.GetCompany(employee.accountId);
                var messageAccount = new MessageAccount(companyId, employee.fullname, employee.email, 3);
                Producer producer = new Producer();
                producer.PublishMessage(JsonConvert.SerializeObject(messageAccount), "AccountGenerate");
                return Ok(rm.Success("Create success"));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("CreateEmployeeExcel")]
        public ActionResult CreateEmployeeExcel([FromBody] List<AccountDTO> employees)
        {
            try
            {
                foreach (var employee in employees) { 
                    int? companyId = AccountDAO.GetCompany(employee.accountId);
                    var messageAccount = new MessageAccount(companyId, employee.fullname, employee.email, 3);
                    Producer producer = new Producer();
                    producer.PublishMessage(JsonConvert.SerializeObject(messageAccount), "AccountGenerate");
                }
                return new JsonResult(rm.Success("Create success"));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
