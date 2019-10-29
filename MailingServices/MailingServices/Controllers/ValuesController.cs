using MailingServices.Model;
using MailingServices.Service;
using Microsoft.AspNetCore.Mvc;

namespace MailingServices.Controllers
{
    [Route("TestSentMailAccount")]
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
