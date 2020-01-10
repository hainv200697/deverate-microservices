using System;
using AuthenServices.Model;
using AuthenServices.Models;
using AuthenServices.RabbitMQ;
using AuthenServices.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AuthenServices.Controllers
{
    [Route("CompanyAPI")]
    public class CompanyController : Controller
    {
        [Route("CreateCompany")]
        [HttpPost]
        public IActionResult PostCreateCompany([FromBody] CompanyDataDTO companyDataDTO)
        {
            try
            {
                if (CompanyDAO.IsExistedCompany(companyDataDTO.CompanyDTO.name))
                {
                    return BadRequest();
                }
                var company = CompanyDAO.CreateCompany(companyDataDTO);
                var account = companyDataDTO.AccountDTO;
                var messageAccount = new MessageAccount(company.CompanyId, account.fullname, account.email, 2, account.address, account.gender, account.phone);
                MessageAccountDTO messageDTO = AccountDAO.GenerateCompanyAccount(messageAccount);
                Producer producer = new Producer();
                producer.PublishMessage(JsonConvert.SerializeObject(messageDTO), "AccountToEmail");
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
