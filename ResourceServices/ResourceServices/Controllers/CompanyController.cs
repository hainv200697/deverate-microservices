using AuthenServices.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ResourceServices.Model;
using ResourceServices.Models;
using ResourceServices.RabbitMQ;
using ResourceServices.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ResourceServices.Controllers
{
    [Route("CompanyAPI")]
    public class CompanyController: Controller
    {
        ResponseMessage rm = new ResponseMessage();
        DeverateContext context;

        public CompanyController(DeverateContext context)
        {
            this.context = context;
        }

        [Route("GetAllCompany")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetAllCompany(bool isActive)
        {
            List<CompanyDTO> com = CompanyDAO.GetAllCompany(isActive);
            return new JsonResult(rm.Success(com));
        }

        [Route("GetAllCompanyById")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetAllCompanyById(int id)
        {
            CompanyDTO com = CompanyDAO.GetCompanyById(id);
            return new JsonResult(rm.Success(com));
        }

        [Route("GetAllCompanyByName")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetAllCompanyByName(string name)
        {
            List<CompanyDTO> com = CompanyDAO.GetCompanyByName(name);
            return new JsonResult(rm.Success(com));
        }

        [Route("CreateCompany")]
        [HttpPost]
        public ActionResult<IEnumerable<string>> PostCreateCompany([FromBody] CompanyDataDTO companyDataDTO)
        {
            try
            {
                var company = CompanyDAO.CreateCompany(companyDataDTO);
                var account = companyDataDTO.AccountDTO;
                var messageAccount = new MessageAccount(company.CompanyId, account.Fullname, account.Email, 2);
                Producer producer = new Producer();
                producer.PublishMessage(message: JsonConvert.SerializeObject(messageAccount), "AccountGenerate");
                return new JsonResult(rm.Success("Save success"));
            } catch
            {
                return new JsonResult(rm.Error("Save fail"));
            }
            
            
        }

        [Route("UpdateCompany")]
        [HttpPut]
        public IActionResult PutUpdateCompany([FromBody] CompanyDTO company)
        {
            string message = CompanyDAO.UpdateCompany(company);
            if (message == null)
            {
                return new JsonResult(rm.Success(message));
            }
            return new JsonResult(rm.Error(message));
        }
    }
}
