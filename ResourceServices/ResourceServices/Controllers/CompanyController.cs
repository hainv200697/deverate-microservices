using AuthenServices.Models;
using Microsoft.AspNetCore.Mvc;
using ResourceServices.Model;
using ResourceServices.Models;
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
                string message = CompanyDAO.CreateCompany(companyDataDTO);
                return Ok(rm.Success(message));
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
