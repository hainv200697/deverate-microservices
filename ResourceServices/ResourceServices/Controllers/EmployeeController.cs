﻿using System;
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

                return Ok(listEmployee);
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

        [HttpPost("ResendPassword")]
        public ActionResult ResendPassword([FromBody] List<string> ListAccountSendPass, int? companyId)
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

        [HttpPut("UpdateEmployeeStatus")]
        public ActionResult UpdateEmployeeStatus([FromBody] List<int> listEmpId,bool? status)
        {
            try
            {
                if (listEmpId == null)
                {
                    return BadRequest();
                }
                var listEmployee = AccountDAO.UpdateEmployeeStatus(listEmpId,status);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }
    }
}
