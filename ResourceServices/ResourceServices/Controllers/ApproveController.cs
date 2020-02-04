using System;
using System.Collections.Generic;
using System.Linq;
using AuthenServices.Model;
using Microsoft.AspNetCore.Mvc;
using ResourceServices.Model;
using ResourceServices.Service;

namespace ResourceServices.Controllers
{
    [Route("api/[controller]")]
    public class ApproveController : Controller
    {
        [HttpGet("GetApproveRequest")]
        public ActionResult GetApproveRequest(int configId)
        {
            try
            {
                List<ApproveRankDTO> requests = ApproveDAO.GetApproveRequest(configId);
                return Ok(requests);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("ApproveOrRejectRank")]
        public ActionResult ApproveOrRejectRank(int testId, bool isApprove)
        {
            try
            {
                ApproveDAO.ActionRequest(testId, isApprove);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

    }
}
