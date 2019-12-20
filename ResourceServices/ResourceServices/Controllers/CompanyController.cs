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
        DeverateContext context;

        public CompanyController(DeverateContext context)
        {
            this.context = context;
        }

        [Route("GetAllCompany")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetAllCompany()
        {
            try
            {
                List<CompanyDTO> com = CompanyDAO.GetAllCompany();
                return Ok(com);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [Route("GetCompanyById")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetAllCompanyById(int id)
        {
            try
            {
                CompanyDataDTO com = CompanyDAO.GetCompanyById(id);
                return Ok(com);
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
                return Ok(com);
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
                if(company == null)
                {
                    return BadRequest();
                }
                CompanyDAO.UpdateCompany(company);
                return Ok(Message.updateCompanySucceed);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [Route("DisableCompany")]
        [HttpPut]
        public IActionResult PutDisableCompany([FromBody] List<int> company, bool? status)
        {
            try
            {
                if(company == null)
                {
                    return BadRequest();
                }
                CompanyDAO.DisableCompany(company, status);
                return Ok(Message.removeCompanySucceed);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
