using AuthenServices.Models;
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
    [Route("ConfigurationApi")]
    public class ConfigurationController: Controller
    {
        ResponseMessage rm = new ResponseMessage();
        DeverateContext context;

        public ConfigurationController(DeverateContext context)
        {
            this.context = context;
        }

        [Route("GetAllConfiguration")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetAllCompany(bool isActive)
        {
            List<ConfigurationDTO> com = ConfigurationDAO.GetAllConfiguration(isActive);
            return new JsonResult(rm.Success(com));
        }

        [Route("CreateConfiguration")]
        [HttpPost]
        public ActionResult<IEnumerable<string>> CreateConfiguration([FromBody] ConfigurationDTO configuration)
        {
            string message = ConfigurationDAO.CreateConfiguration(configuration);
            if (message == null)
            {
                return new JsonResult(rm.Success(message));
            }
            return new JsonResult(rm.Error(message));
        }
    }
}
