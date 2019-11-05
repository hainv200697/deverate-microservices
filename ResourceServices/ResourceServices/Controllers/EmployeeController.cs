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
        public ActionResult CreateEmployee([FromBody] List<MessageAccount> employee)
        {
            try
            {
                foreach(var emp in employee)
                {
                    bool check = AccountDAO.IsEmailUnique(emp.Email);
                    if (check)
                    {
                        return BadRequest("Email is existed");
                    }
                }
                Producer producer = new Producer();
                producer.PublishMessage(JsonConvert.SerializeObject(employee), "AccountGenerate");
                return Ok(rm.Success("Create success"));
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

                return Ok(rm.Success(listEmployee));
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }
    }
}
