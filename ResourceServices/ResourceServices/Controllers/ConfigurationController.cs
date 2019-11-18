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
        public ActionResult GetAllCompany(bool isActive, int companyId)
        {
            try
            {
                List<ConfigurationDTO> con = ConfigurationDAO.GetAllConfiguration(isActive, companyId);
                return Ok(con);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [Route("GetConfigurationById")]
        [HttpGet]
        public ActionResult GetConfigurationById(int id)
        {
            try
            {
                ConfigurationDTO con = ConfigurationDAO.GetConfigurationById(id);
                return Ok(con);
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
                if(configuration.type.Value)
                {
                    var save = ConfigurationDAO.CreateConfiguration(configuration);
                    if (save == null)
                    {
                        return BadRequest("No employee");
                    }
                    Producer producer = new Producer();
                    producer.PublishMessage(save.configId + "", "GenerateTest");
                    return Ok(rm.Success("Save success"));
                }
                else
                {
                    ConfigurationDAO.CreateConfiguration(configuration);
                    return Ok(rm.Success("Save success"));
                }
            }
            catch (Exception ex)
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
                return Ok(rm.Success(message));
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
                return Ok(rm.Success(message));
            }
            catch(Exception)
            {
                return StatusCode(500);
            }
            
        }
    }
}
