using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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
        ResponseMessage rm = new ResponseMessage();
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
        public ActionResult<IEnumerable<string>> PostCreateManagerAccount([FromBody]MessageAccount account)
        {
            var result = AccountDAO.GenerateCompanyAccount(context, account).Split('_');
            Producer producer = new Producer();
            MessageAccountDTO messageDTO = new MessageAccountDTO(result[0], result[1], account.Email, account.Fullname);
            producer.PublishMessage(message: JsonConvert.SerializeObject(messageDTO), "AccountToEmail");
            return new JsonResult(rm.Success("Login successful", result));
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
            catch (Exception exe)
            {
                return StatusCode(500);
            }
        }
    }
}
