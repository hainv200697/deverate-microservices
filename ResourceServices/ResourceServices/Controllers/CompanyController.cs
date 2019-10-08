using AuthenServices.Models;
using Microsoft.AspNetCore.Mvc;
using ResourceServices.Model;
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
        public IActionResult GetAllCompany(bool isActive)
        {
            List<CompanyDTO> com = CompanyDAO.GetAllCompany(isActive);
            return Ok(com);
        }

        [Route("GetAllCompanyById")]
        [HttpGet]
        public IActionResult GetAllCompanyById(int id)
        {
            CompanyDTO com = CompanyDAO.GetCompanyById(id);
            return Ok(com);
        }

        [Route("GetAllCompanyByName")]
        [HttpGet]
        public IActionResult GetAllCompanyByName(string name)
        {
            List<CompanyDTO> com = CompanyDAO.GetCompanyByName(name);
            return Ok(com);
        }

        [Route("CreateCompany")]
        [HttpPost]
        public IActionResult PostCreateCompany([FromBody] CompanyDTO company)
        {
            string message = CompanyDAO.CreateCompany(company);
            if (message == null)
            {
                return Ok(message);
            }
            return StatusCode(0);
        }

        [Route("UpdateCompany")]
        [HttpPut]
        public IActionResult PutUpdateCompany([FromBody] CompanyDTO company)
        {
            string message = CompanyDAO.UpdateCompany(company);
            if (message == null)
            {
                return Ok(message);
            }
            return StatusCode(0);
        }
    }
}
