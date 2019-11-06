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
                return BadRequest();
            }
        }



        [HttpPost]
        [Route("CreateCatalogue")]
        public ActionResult CreateCatalogue([FromBody]CatalogueDTO catalog)
        {
            try
            {
                var message = CatalogueDAO.CreateCatalogue(catalog);
                return Ok(message);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("UpdateCatalogue")]
        public ActionResult UpdateCatalogue([FromBody]CatalogueDTO catalog)
        {
            try
            {
                var message = CatalogueDAO.UpdateCatalogueDefault(catalog);
                return Ok(message);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("RemoveCatalogue")]
        public ActionResult RemoveCatalogue([FromBody]List<CatalogueDTO> catalog)
        {
            try
            {
                var message = CatalogueDAO.removeCatalogueDefault(catalog);
                return Ok(message);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("GetAllCatalogueDefault")]
        public ActionResult GetAllCatalogueDefault(bool status)
        {
            try
            {
                List<CatalogueDTO> catalogues = CatalogueDAO.GetAllCatalogueDefault(status);
                return Ok(catalogues);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }



        [HttpPost("CreateCatalogueDefault")]
        public ActionResult CreateCatalogueDefault([FromBody]CatalogueDTO catalog)
        {
            try
            {
                var message = CatalogueDAO.CreateCatalogueDefault(catalog);
                return Ok(message);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut("UpdateCatalogueDefault")]
        public ActionResult UpdateCatalogueDefault([FromBody]CatalogueDTO catalog)
        {
            try
            {
                var message = CatalogueDAO.UpdateCatalogueDefault(catalog);
                return Ok(message);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut("RemoveCatalogueDefault")]
        public ActionResult RemoveCatalogueDefault([FromBody]List<CatalogueDTO> catalog)
        {
            try
            {
                var message = CatalogueDAO.removeCatalogueDefault(catalog);
                return Ok(message);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
