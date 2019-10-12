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

    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        ResponseMessage rm = new ResponseMessage();
        DeverateContext context;
        public LoginController(DeverateContext context)
        {
            this.context = context;
        }

        [HttpPost("Authen")]
        public ActionResult<IEnumerable<string>> PostAuthenUser([FromBody]AccountDTO account)
        {

            string token = AccountDAO.CheckLogin(context, account.Username, account.Password);
            if (token == null)
            {
                return new JsonResult(rm.Error("Invalid username or password"));
            }
            return new JsonResult(rm.Success("Login successful", token));
        }

        [HttpPost("CreateManagerAccount")]
        public ActionResult<IEnumerable<string>> PostCreateManagerAccount([FromBody]MessageAccount account)
        {
            var result = AccountDAO.GenerateCompanyAccount(context, account).Split('_');
            Producer producer = new Producer();
            MessageAccountDTO messageDTO = new MessageAccountDTO(result[0], result[1], account.Email);
            producer.PublishMessage(message: JsonConvert.SerializeObject(messageDTO), "AccountToEmail");
            return new JsonResult(rm.Success("Login successful", result));
        }
    }
}
