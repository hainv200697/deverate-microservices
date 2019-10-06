using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenServices.Models;
using Microsoft.AspNetCore.Mvc;
using TestManagementServices.Model;
using TestManagementServices.Models;
using TestManagementServices.Service;

namespace TestManagementServices.Controllers
{
    [Route("api/[controller]")]
    public class SystemController : Controller
    {
        ResponseMessage rm = new ResponseMessage();
        DeverateContext context;

        public SystemController(DeverateContext context)
        {
            this.context = context;
        }

        [HttpPost("EvaluateRank")]
        public ActionResult<IEnumerable<string>> PostEvaluateRank([FromBody]TestAnswerDTO answer)
        {

            RankPoint rp = SystemDAO.EvaluateRank(context, answer);
            if(rp == null)
            {
                return new JsonResult(rm.Error("Evaluate Failed"));
            }
            return new JsonResult(rm.Success("Evaluate successful", rp));
        }
    }
}
