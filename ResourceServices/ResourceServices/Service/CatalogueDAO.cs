using ResourceServices.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenServices.Models;
using ResourceServices.Models;
using Microsoft.EntityFrameworkCore;

namespace ResourceServices.Service
{
    public class CatalogueDAO
    {
        public static List<CatalogueDTO> GetAllCatalogue(int companyId ,bool status)
        {
            using (DeverateContext context = new DeverateContext())
            {
                
                List<CatalogueDTO> catalogues = new List<CatalogueDTO>();
                var cata = context.Catalogue.Include(x=>x.Question).Where(x => x.IsActive == status && x.Type == false).ToList();
                foreach (var item in cata)
                {
                    CatalogueDTO catalogue = new CatalogueDTO(item, companyId);
                    catalogues.Add(catalogue);
                }
                if (companyId != null)
                {
                var CompanyCatalogue = context.CompanyCatalogue
                        .Include(x => x.Catalogue)
                        .Where(x => x.IsActive == status && x.CompanyId == companyId)
                        .ToList();
                    foreach (var item in CompanyCatalogue)
                    {
                        CatalogueDTO catalogue = new CatalogueDTO(item.Catalogue, companyId);
                        catalogues.Add(catalogue);
                    }
                }
                return catalogues;
               
            }

        }



        public static string CreateCatalogue(CatalogueDTO catalogue)
        {
            using (DeverateContext context = new DeverateContext())
            {
                Catalogue cata = new Catalogue();
                cata.Description = catalogue.description;
                cata.Name = catalogue.name;
                cata.IsActive = catalogue.isActive;
                if (catalogue.companyId != null)
                {
                    cata.Type = true;
                }
                else
                {
                    cata.Type = false;
                }
                cata.IsActive = true;
                context.Catalogue.Add(cata);
                if(catalogue.companyId != null) { 
                    CompanyCatalogue comcata = new CompanyCatalogue();
                    comcata.CompanyId = catalogue.companyId;
                    comcata.CatalogueId = cata.CatalogueId;
                    comcata.IsActive = true;
                    context.CompanyCatalogue.Add(comcata);
                }
                context.SaveChanges();
                return Message.createCatalogueSucceed;
            }

        }

        public static string UpdateCatalogue(CatalogueDTO catalogue)
        {
            using (DeverateContext context = new DeverateContext())
            {
                Catalogue cata = context.Catalogue.SingleOrDefault(c => c.CatalogueId == catalogue.catalogueId);
                cata.Description = catalogue.description;
                cata.Name = catalogue.name;
                cata.IsActive = catalogue.isActive;
                context.SaveChanges();
                return Message.updateCatalogueSucceed; 
            }

        }

        public static string removeCatalogue(List<CatalogueDTO> catalogue)
        {
            using (DeverateContext context = new DeverateContext())
            {
                foreach (var cata in catalogue)
                {
                    Catalogue cataDb = context.Catalogue.SingleOrDefault(c => c.CatalogueId == cata.catalogueId);
                    cataDb.IsActive = cata.isActive;
                    if(cata.companyId != null) {
                        CompanyCatalogue comcata = context.CompanyCatalogue.SingleOrDefault(c => c.CatalogueId == cata.catalogueId && c.CompanyId == cata.companyId);
                        comcata.IsActive = cata.isActive;
                    }
                    context.SaveChanges();
                }
                return Message.removeCatalogueSucceed;
            }
        }
    }
}
