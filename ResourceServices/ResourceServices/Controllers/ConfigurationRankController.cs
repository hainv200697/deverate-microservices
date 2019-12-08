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
    [Route("ConfigurationRankController")]
    public class ConfigurationRankController: Controller
    {

        [Route("GetAllConfigurationRank")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetAllConfigurationRank(bool isActive)
        {
            List<ConfigurationRankDTO> com = ConfigurationRankDAO.GetAllRank(isActive);
            return new JsonResult(com);
        }
    }
}
