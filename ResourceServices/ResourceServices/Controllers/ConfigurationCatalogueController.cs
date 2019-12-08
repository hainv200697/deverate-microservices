using AuthenServices.Models;
using Microsoft.AspNetCore.Mvc;
using ResourceServices.Model;
using ResourceServices.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Controllers
{
    [Route("ConfigurationCatalogue")]
    public class ConfigurationCatalogueController: Controller
    {

        [Route("GetAllConfigurationCatalogue")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetAllConfigurationCatalogue(bool isActive)
        {
            List<ConfigurationCatalogueDTO> com = ConfigurationCatalogueDAO.GetAllConfigurationCatalogue(isActive);
            return new JsonResult(com);
        }

        [Route("GetConfigurationCatalogueByConfigId")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetConfigurationCatalogueByConfigId(int id)
        {
            List<ConfigurationCatalogueDTO> com = ConfigurationCatalogueDAO.GetConfigurationCatalogueByConfigId(id);
            return new JsonResult(com);
        }
    }
}
