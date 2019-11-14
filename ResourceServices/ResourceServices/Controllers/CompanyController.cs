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
            try
            {
                List<CompanyDTO> com = CompanyDAO.GetAllCompany(isActive);
                return Ok(rm.Success(com));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [Route("GetAllCompanyById")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetAllCompanyById(int id)
        {
            try
            {
                CompanyDTO com = CompanyDAO.GetCompanyById(id);
                return Ok(rm.Success(com));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [Route("GetAllCompanyByName")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetAllCompanyByName(string name)
        {
            try
            {
                List<CompanyDTO> com = CompanyDAO.GetCompanyByName(name);
                return Ok(rm.Success(com));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [Route("CreateCompany")]
        [HttpPost]
        public ActionResult<IEnumerable<string>> PostCreateCompany([FromBody] CompanyDataDTO companyDataDTO)
        {
            try
            {
                var company = CompanyDAO.CreateCompany(companyDataDTO);
                var account = companyDataDTO.AccountDTO;
                var messageAccount = new MessageAccount(company.CompanyId, account.fullname, account.email, 2,account.address,account.gender,account.phone);
                Producer producer = new Producer();
                producer.PublishMessage(JsonConvert.SerializeObject(messageAccount), "AccountGenerate");

                return new JsonResult(rm.Success("Save success"));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [Route("UpdateCompany")]
        [HttpPut]
        public IActionResult PutUpdateCompany([FromBody] CompanyDTO company)
        {
            try
            {
                string message = CompanyDAO.UpdateCompany(company);
                return Ok(rm.Success(message));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [Route("DisableCompany")]
        [HttpPut]
        public IActionResult PutDisableCompany([FromBody] List<CompanyDTO> company)
        {
            try
            {
                string message = CompanyDAO.DisableCompany(company);
                return Ok(rm.Success(message));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
