using Microsoft.AspNetCore.Mvc;
using ResourceServices.Model;
using ResourceServices.Models;
using ResourceServices.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Controllers
{
    [Route("SemesterAPI")]
    public class SemesterController : Controller
    {
        DeverateContext context;

        public SemesterController(DeverateContext context)
        {
            this.context = context;
        }

        [Route("GetAllEmployeeInCompany")]
        [HttpGet]
        public ActionResult GetAllEmployeeInCompany(int companyId)
        {
            try
            {
                List<SemesterDTO> emp = SemesterDAO.getAllEmployeeInCompany(companyId);
                return Ok(emp);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
