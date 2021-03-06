﻿using Microsoft.AspNetCore.Mvc;
using ResourceServices.Model;
using ResourceServices.Service;
using System;
using System.Collections.Generic;

namespace ResourceServices.Controllers
{
    [Route("ConfigurationApi")]
    public class ConfigurationController : Controller
    {
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

        [Route("CloneConfiguration")]
        [HttpPost]
        public ActionResult CloneConfiguration(int configId, String title)
        {
            try
            {
                ConfigurationDAO.CloneConfiguration(configId, title);
                return Ok(Message.createConfigSucceed);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [Route("ChangeStatusConfiguration")]
        [HttpPut]
        public IActionResult ChangeStatusConfiguration([FromBody] List<int> configIds, bool isActive)
        {
            try
            {
                ConfigurationDAO.ChangeStatusConfiguration(configIds, isActive);
                return Ok(Message.changeStatusConfigSucceed);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

        }

    }
}
