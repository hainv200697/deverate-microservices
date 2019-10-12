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
        ResponseMessage rm = new ResponseMessage();

        [Route("GetAllRank")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetAllRank(bool isActive)
        {
            List<RankDTO> com = RankDAO.GetAllRank(isActive);
            return new JsonResult(rm.Success(com));
        }
    }
}
