using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailingServices.Models;
using MailingServices.Service;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MailingServices.Controllers
{
    [Route("TestSentMail")]
    public class ValuesController : Controller
    {

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]MessageAccountDTO messageAccountDTO)
        {
            EmailSender.SendAccountMailAsync(messageAccountDTO);
        }
    }
}
