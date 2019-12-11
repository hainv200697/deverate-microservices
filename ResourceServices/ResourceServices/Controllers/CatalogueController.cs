using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AuthenServices.Models;
using System.Net.Http;
using ResourceServices.Model;
using ResourceServices.Service;
using System.Net;
using ResourceServices.Models;

namespace ResourceServices.Controllers
{
    [Route("api/[controller]")]
    public class CatalogueController : Controller
    {
        [HttpGet("GetAllCatalogue")]
        public ActionResult GetAllCatalogue(int companyId, bool status)
        {
            try
            {
                List<CatalogueDTO> catalogues = CatalogueDAO.GetAllCatalogue(companyId, status);
                if (catalogues == null)
                {
                    return BadRequest();
                }
                return Ok(catalogues);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }



        [HttpPost]
        [Route("CreateCatalogue")]
        public ActionResult CreateCatalogue([FromBody]CatalogueDTO catalog)
        {
            try
            {
                if (catalog == null)
                {
                    return BadRequest();
                }
                CatalogueDAO.CreateCatalogue(catalog);
                return Ok(Message.createCatalogueSucceed);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut]
        [Route("UpdateCatalogue")]
        public ActionResult UpdateCatalogue([FromBody]CatalogueDTO catalog)
        {
            try
            {
                if (catalog == null)
                {
                    return BadRequest();
                }
                CatalogueDAO.UpdateCatalogue(catalog);
                return Ok(Message.updateCatalogueSucceed);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut]
        [Route("RemoveCatalogue")]
        public ActionResult RemoveCatalogue([FromBody]List<CatalogueDTO> catalog)
        {
            try
            {
                if (catalog == null)
                {
                    return BadRequest();
                }
                CatalogueDAO.removeCatalogue(catalog);
                return Ok(Message.removeCatalogueSucceed);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("GetAllCatalogueDefault")]
        public ActionResult GetAllCatalogueDefault(bool status)
        {
            try
            {
                List<CatalogueDefaultDTO> catalogues = CatalogueDAO.GetAllCatalogueDefault(status);
                return Ok(catalogues);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }



        [HttpPost("CreateCatalogueDefault")]
        public ActionResult CreateCatalogueDefault([FromBody]CatalogueDefaultDTO catalog)
        {
            try
            {
                if (catalog == null)
                {
                    return BadRequest();
                }
                CatalogueDAO.CreateCatalogueDefault(catalog);
                return Ok(Message.createCatalogueSucceed);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("UpdateCatalogueDefault")]
        public ActionResult UpdateCatalogueDefault([FromBody]CatalogueDefaultDTO catalog)
        {
            try
            {
                if (catalog == null)
                {
                    return BadRequest();
                }
                CatalogueDAO.UpdateCatalogueDefault(catalog);
                return Ok(Message.updateCatalogueSucceed);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("RemoveCatalogueDefault")]
        public ActionResult RemoveCatalogueDefault([FromBody]List<CatalogueDefaultDTO> catalog)
        {
            try
            {
                if (catalog == null)
                {
                    return BadRequest();
                }
                CatalogueDAO.removeCatalogueDefault(catalog);
                return Ok(Message.removeCatalogueSucceed);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
