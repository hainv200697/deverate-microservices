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
        ResponseMessage _rm = new ResponseMessage();
        DeverateContext context;

        [HttpGet]
        [Route("GetAllCatelogue")]
        public ActionResult GetAllCatelogue()
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

        [HttpPost]
        [Route("CreateCatelogue")]
        public ActionResult CreateCatelogue(CatalogueDTO catalog)
        {
            try
            {
                string message = CatalogueDAO.CreateCatalogue(catalog);
                return Ok(message);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpPost]
        [Route("UpdateCatelogue")]
        public ActionResult UpdateCatelogue(CatalogueDTO catalog)
        {
            try
            {
                string message = CatalogueDAO.UpdateCatalogue(catalog);
                return Ok(message);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("RemoveCatelogue")]
        public ActionResult RemoveCatelogue(List<CatalogueDTO> catalog)
        {
            try
            {
                string message = CatalogueDAO.removeCatalogue(catalog);
                return Ok(message);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
