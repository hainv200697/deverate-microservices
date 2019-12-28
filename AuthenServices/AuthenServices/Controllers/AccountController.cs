using System;
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
        [HttpPost("Login")]
        public ActionResult<IEnumerable<string>> PostAuthenUser([FromBody]AccountLoginDTO account)
        {
            try
            {
                string token = AccountDAO.CheckLogin(account.username, account.password);
                if (token == null)
                {
                    return BadRequest("Invalid username or password");
                }
                return Ok(new TokenResponse { Token = token });
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error " + ex);
                return StatusCode(500);
            }
        }

        [HttpPut("ChangePassword")]
        public ActionResult PutChangePassword([FromBody]ChangePassRequest changePassRequest)
        {
            try
            {
                bool result = AccountDAO.ChangePassword(changePassRequest);
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
                var listResendAccount = AccountDAO.Resend(listUsername);
                Producer producer = new Producer();
                foreach (MessageAccountDTO msAccount in listResendAccount)
                {
                    producer.PublishMessage(message: JsonConvert.SerializeObject(msAccount), "AccountToEmail");
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
