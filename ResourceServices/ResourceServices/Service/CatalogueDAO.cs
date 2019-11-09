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
                var catalouge = context.CatalogueInCompany.Include(x=>x.Catalogue).Include(x=>x.Question).Where(x => x.IsActive == status && x.CompanyId == companyId)
                    .Select(x=> new CatalogueDTO(x.Catalogue,x.IsActive,x.Question.Count(ques => ques.IsActive == true)))
                    .ToList();
                return catalouge;
            }
        }

        public static List<CatalogueDTO> GetAllCatalogueDefault( bool status)
        {
            using (DeverateContext context = new DeverateContext())
            {
                List<CatalogueDTO> catalogues = new List<CatalogueDTO>();
                var cata = context.Catalogue.Where(x => x.IsActive == status && x.Type == false).Select(x=> new CatalogueDTO(x)).ToList();
                return cata;
            }
        }

        public static void CreateCatalogue(CatalogueDTO catalogue)
        {
            using (DeverateContext context = new DeverateContext())
            {
                Catalogue cata = new Catalogue();
                cata.Description = catalogue.description;
                cata.Name = catalogue.name;
                cata.IsActive = catalogue.isActive;
                cata.Type = true;
                cata.IsActive = true;
                context.Catalogue.Add(cata);
                CatalogueInCompany cataCom = new CatalogueInCompany();
                cataCom.CatalogueId = cata.CatalogueId;
                cataCom.CompanyId = catalogue.companyId;
                cataCom.IsActive = true;
                context.CatalogueInCompany.Add(cataCom);
                context.SaveChanges();
            }

        }

        public static void CreateCatalogueDefault(CatalogueDTO catalogue)
        {
            using (DeverateContext context = new DeverateContext())
            {
                Catalogue cata = new Catalogue();
                cata.Description = catalogue.description;
                cata.Name = catalogue.name;
                cata.IsActive = catalogue.isActive;
                cata.Type = false;
                cata.IsActive = true;
                context.Catalogue.Add(cata);
                context.SaveChanges();
            }

        }

        public static void UpdateCatalogueDefault(CatalogueDTO catalogue)
        {
            using (DeverateContext context = new DeverateContext())
            {
                Catalogue cata = context.Catalogue.SingleOrDefault(c => c.CatalogueId == catalogue.catalogueId);
                cata.Description = catalogue.description;
                cata.Name = catalogue.name;
                cata.IsActive = catalogue.isActive;
                context.SaveChanges();
            }

        }

        public static void removeCatalogueDefault(List<CatalogueDTO> catalogue)
        {
            using (DeverateContext context = new DeverateContext())
            {
                foreach (var cata in catalogue)
                {
                    CatalogueInCompany cataDb = context.CatalogueInCompany.SingleOrDefault(c => c.CatalogueId == cata.catalogueId && c.CompanyId == cata.companyId);
                    cataDb.IsActive = cata.isActive.Value;
                }
                context.SaveChanges();
            }
        }
    }
}
