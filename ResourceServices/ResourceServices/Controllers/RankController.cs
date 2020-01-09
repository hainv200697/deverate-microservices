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
    [Route("RankApi")]
    public class RankController: Controller
    {

        [Route("GetAllCompanyRank")]
        [HttpGet]
        public IActionResult GetAllRank(bool isActive, int companyId)
        {
            try
            {
                List<CompanyRankDTO> rank = RankDAO.getAllCompanyRank(isActive, companyId);
                return Ok(rank);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [Route("GetAllDefaultRank")]
        [HttpGet]
        public IActionResult GetAllDefaultRank()
        {
            try
            {
                RankDAO.UpdateRalationIfNot();
                ListRankAndListCatalogueDTO rank = RankDAO.getAllDefaultRank();
                return Ok(rank);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [Route("SaveDefaultRank")]
        [HttpPost]
        public IActionResult SaveDefaultRank([FromBody] List<DefaultRankDTO> defaultRankDTOs)
        {
            try
            {
                RankDAO.SaveDefaultRank(defaultRankDTOs);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [Route("DisableDefaultRank")]
        [HttpPost]
        public IActionResult DisableDefaultRank([FromBody] List<int> ids)
        {
            try
            {
                RankDAO.DisableDefaultRank(ids);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
