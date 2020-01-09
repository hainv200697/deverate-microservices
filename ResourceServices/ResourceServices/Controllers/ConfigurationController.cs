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
    public class ConfigurationController : Controller
    {
        DeverateContext context;

        public ConfigurationController(DeverateContext context)
        {
            this.context = context;
        }

        [Route("GetAllConfiguration")]
        [HttpGet]
        public ActionResult GetAllCompany(bool type, int companyId)
        {
            try
            {
                List<ConfigurationDTO> con = ConfigurationDAO.GetAllConfiguration(type, companyId);
                return Ok(con);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [Route("GetConfigurationForApplicant")]
        [HttpGet]
        public ActionResult GetConfigurationForApplicant(bool type, int companyId)
        {
            try
            {
                List<ConfigurationDTO> con = ConfigurationDAO.GetAllConfigurationForApplicant(type, companyId);
                return Ok(con);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [Route("GetConfigurationForEmployee")]
        [HttpGet]
        public ActionResult GetConfigurationForEmployee(int companyId)
        {
            try
            {
                List<ConfigurationDTO> con = ConfigurationDAO.GetConfigurationForEmployee(companyId);
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
                ConfigurationDAO.CreateConfiguration(configuration);
                return Ok(Message.createConfigSucceed);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }


        [Route("UpdateConfiguration")]
        [HttpPut]
        public ActionResult UpdateConfiguration([FromBody] ConfigurationDTO configuration)
        {
            try
            {
                ConfigurationDAO.UpdateConfiguration(configuration);
                return Ok(Message.updateConfigSucceed);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [Route("ChangeStatusConfiguration")]
        [HttpPut]
        public ActionResult ChangeStatusConfiguration([FromBody] List<int> configId, bool isActive)
        {
            try
            {
                ConfigurationDAO.ChangeStatusConfiguration(configId, isActive);
                return Ok(Message.changeStatusConfigSucceed);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

        }
    }
}
