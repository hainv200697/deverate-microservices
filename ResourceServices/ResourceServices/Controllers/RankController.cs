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

        [Route("ChangeStatusCompanyRank")]
        [HttpPut]
        public IActionResult ChangeStatusCompanyRank([FromBody] List<int> rankId, bool status)
        {
            try
            {
                RankDAO.changeStatusCompanyRank(rankId,status);
                return Ok(Message.changeStatusCompanyRankSucceed);
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
     
    }
}
