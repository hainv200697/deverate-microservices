using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AuthenServices.Model;
using AuthenServices.Models;
using AuthenServices.Service;
using Microsoft.AspNetCore.Mvc;


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
            if(token == null)
            {
                return new JsonResult(rm.Error("Invalid username or password"));
            }
            return new JsonResult(rm.Success("Login successful", token));
        }

        [HttpPost("CreateManagerAccount")]
        public ActionResult<IEnumerable<string>> PostCreateManagerAccount([FromBody]CompanyManagerDTO account)
        {

            var result = AccountDAO.CreateCompanyAccount(context, account).Split('_');
            return new JsonResult(rm.Success("Login successful", result));
        }
    }
}
