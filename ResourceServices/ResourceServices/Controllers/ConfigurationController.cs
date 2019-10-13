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

        [Route("GetConfigurationById")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetConfigurationById(int id)
        {
            ConfigurationDTO com = ConfigurationDAO.GetConfigurationById(id);
            return new JsonResult(rm.Success(com));
        }

        [Route("CreateConfiguration")]
        [HttpPost]
        public ActionResult CreateConfiguration([FromBody] ConfigurationDTO configuration)
        {
            try
            {
                var message = ConfigurationDAO.CreateConfiguration(configuration);
                return Ok(message);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
            

        [Route("UpdateConfiguration")]
        [HttpPut]
        public ActionResult<IEnumerable<string>> UpdateConfiguration([FromBody] ConfigurationDTO configuration)
        {
            try
            {
                var message = ConfigurationDAO.UpdateConfiguration(configuration);
                return Ok(message);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            
            
        }

        [Route("ChangeStatusConfiguration")]
        [HttpPut]
        public ActionResult<IEnumerable<string>> ChangeStatusConfiguration([FromBody] List<ConfigurationDTO> configuration)
        {
            try
            {
                var message = ConfigurationDAO.ChangeStatusConfiguration(configuration);
                return Ok(message);
            }
            catch(Exception)
            {
                return BadRequest();
            }
            
        }
    }
}
