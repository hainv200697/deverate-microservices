﻿using System;
using System.Collections.Generic;
using AuthenServices.Model;
using AuthenServices.Models;
using AuthenServices.RabbitMQ;
using AuthenServices.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AuthenServices.Controllers
{

    [Route("[controller]")]
    public class AccountController : Controller
    {
        DeverateContext context;
        public AccountController(DeverateContext context)
        {
            this.context = context;
        }

        [HttpPost("Login")]
        public ActionResult<IEnumerable<string>> PostAuthenUser([FromBody]AccountDTO account)
        {

            string token = AccountDAO.CheckLogin(context, account.Username, account.Password);
            if (token == null)
            {
                return BadRequest("Invalid username or password");
            }
            return Ok(new TokenResponse { Token = token});
        }

        [HttpPost("CreateManagerAccount")]
        public ActionResult PostCreateManagerAccount([FromBody]MessageAccount account)
        {
            Producer producer = new Producer();
            MessageAccountDTO messageDTO = AccountDAO.GenerateCompanyAccount(context, account);
            producer.PublishMessage(message: JsonConvert.SerializeObject(messageDTO), "AccountToEmail");
            return Ok(messageDTO);
        }

        [HttpPut("ChangePassword")]
        public ActionResult PutChangePassword([FromBody]ChangePassRequest changePassRequest)
        {
            try
            {
                bool result = AccountDAO.changePassword(changePassRequest);
                if (result)
                {
                    return Ok();
                }
                return BadRequest("Old password is invalid");
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("resend")]
        public ActionResult ResendPassword([FromBody]List<String> listUsername)
        {
            try
            {
                AccountDAO.resend(listUsername);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
