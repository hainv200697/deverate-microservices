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
            try
            {
                List<ConfigurationDTO> con = ConfigurationDAO.GetAllConfiguration(isActive);
                return Ok(rm.Success(con));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [Route("GetConfigurationById")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetConfigurationById(int id)
        {
            try
            {
                ConfigurationDTO con = ConfigurationDAO.GetConfigurationById(id);
                return Ok(rm.Success(con));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [Route("CreateConfiguration")]
        [HttpPost]
        public ActionResult CreateConfiguration([FromBody] ConfigurationDTO configuration)
        {
            try
            {
                var message = ConfigurationDAO.CreateConfiguration(configuration);
                return Ok(rm.Success(message));
            }
            catch (Exception)
            {
                return StatusCode(500);
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
                return StatusCode(500);
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
                return StatusCode(500);
            }
            
        }
    }
}
