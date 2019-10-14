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
        [HttpGet]
        [Route("GetAllCatalogue")]
        public ActionResult GetAllCatalogue()
        {
            try
            {
                List<CatalogueDTO> catalogues = CatalogueDAO.GetAllCatalogue();
                return Ok(catalogues);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("GetCatalogueById")]
        public ActionResult GetCatalogueById(int id)
        {
            try
            {
                CatalogueDTO catalogues = CatalogueDAO.GetCatalogueById(id);
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
                var message = CatalogueDAO.UpdateCatalogue(catalog);
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
                var message = CatalogueDAO.removeCatalogue(catalog);
                return Ok(message);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
