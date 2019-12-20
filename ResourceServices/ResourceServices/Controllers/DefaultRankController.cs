using Microsoft.AspNetCore.Mvc;
using ResourceServices.Model;
using ResourceServices.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Controllers
{
    [Route("DefaultRankApi")]
    public class DefaultRankController: Controller
    {
        [Route("GetAllDefaultRank")]
        [HttpGet]
        public IActionResult GetAllDefaultRank(bool isActive)
        {
            try
            {
                List<DefaultRankDTO> rank = DefaultRankDAO.getAllDefaultRank(isActive);
                return Ok(rank);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
        [Route("CreateDefaultRank")]
        [HttpPost]
        public IActionResult CreateDefaultRank([FromBody] DefaultRankDTO defaultRankDTO)
        {
            try
            {
                if (defaultRankDTO == null)
                {
                    return BadRequest(Message.inputDefaultRank);
                }
                if(DefaultRankDAO.checkExistedRank(defaultRankDTO.name) == true)
                {
                    return BadRequest(Message.DefaultRankExisted);
                }
                DefaultRankDAO.createDefaultRank(defaultRankDTO);
                return Ok(Message.createDefaultRankSucceed);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
        [Route("UpdateDefaultRank")]
        [HttpPut]
        public IActionResult UpdateDefaultRank([FromBody] DefaultRankDTO defaultRankDTO)
        {
            try
            {
                if (defaultRankDTO == null)
                {
                    return BadRequest(Message.inputDefaultRank);
                }
                if (DefaultRankDAO.checkExistedRank(defaultRankDTO.name) == true)
                {
                    return BadRequest(Message.DefaultRankExisted);
                }
                DefaultRankDAO.updateDefaultRank(defaultRankDTO);
                return Ok(Message.updateDefaultRankSucceed);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
        [Route("ChangeStatusDefaultRank")]
        [HttpPut]
        public IActionResult ChangeStatusDefaultRank([FromBody] List<int> defaultRankId, bool status)
        {
            try
            {
                if(defaultRankId == null || defaultRankId.Count == 0)
                {
                    return BadRequest(Message.chooseDefaultRank);
                }
                DefaultRankDAO.changeStatusDefaultRank(defaultRankId, status);
                return Ok(Message.changeStatusDefaultRankSucceed);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
