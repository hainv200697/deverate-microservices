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
                var catalouge = context.CompanyCatalogue.Include(x=>x.Question).Where(x => x.IsActive == status && x.CompanyId == companyId)
                    .Select(x=> new CatalogueDTO(x,x.IsActive,x.Question.Count(ques => ques.IsActive == true)))
                    .ToList();
                return catalouge;
            }
        }

        public static List<CatalogueDefaultDTO> GetAllCatalogueDefault( bool status)
        {
            using (DeverateContext context = new DeverateContext())
            {
                var cata = context.CompanyCatalogue
                    .Include(x=> x.Question)
                    .Where(x => x.IsActive == status && x.Type == true)
                    .Select(x=> new CatalogueDefaultDTO(x, x.Question.Count(ques => ques.IsActive == true))).ToList();
                return cata;
            }
        }

        public static void CreateCatalogue(CatalogueDTO catalogue)
        {
            using (DeverateContext context = new DeverateContext())
            {
                CompanyCatalogue cata = new CompanyCatalogue();
                cata.CompanyId = catalogue.companyId;
                cata.Description = catalogue.description;
                cata.Name = catalogue.name;
                cata.IsActive =true;
                cata.Type = false;
                cata.CreateDate = DateTime.UtcNow;
                context.CompanyCatalogue.Add(cata);
                context.SaveChanges();
            }

        }

        public static void CreateCatalogueDefault(CatalogueDefaultDTO catalogue)
        {
            using (DeverateContext context = new DeverateContext())
            {
                CompanyCatalogue cata = new CompanyCatalogue
                {
                    Description = catalogue.description,
                    Name = catalogue.name,
                    IsActive = true,
                    CreateDate = DateTime.UtcNow,
                    Type = true
                };
                context.CompanyCatalogue.Add(cata);
                context.SaveChanges();
            }

        }

        public static void UpdateCatalogue(CatalogueDTO catalogue)
        {
            using (DeverateContext context = new DeverateContext())
            {
                CompanyCatalogue cata = context.CompanyCatalogue.SingleOrDefault(c => c.CompanyCatalogueId == catalogue.companyCatalogueId);
                cata.Description = catalogue.description;
                cata.Name = catalogue.name;
                cata.IsActive = catalogue.isActive;
                context.SaveChanges();
            }

        }

        public static void UpdateCatalogueDefault(CatalogueDefaultDTO catalogue)
        {
            using (DeverateContext context = new DeverateContext())
            {
                CompanyCatalogue cata = context.CompanyCatalogue.SingleOrDefault(c => c.CompanyCatalogueId == catalogue.catalogueId);
                cata.Description = catalogue.description;
                cata.Name = catalogue.name;
                cata.IsActive = catalogue.isActive;
                context.SaveChanges();
            }

        }

        public static void removeCatalogueDefault(List<CatalogueDefaultDTO> catalogue)
        {
            using (DeverateContext context = new DeverateContext())
            {
                foreach (var cata in catalogue)
                {
                    CompanyCatalogue cataDb = context.CompanyCatalogue.SingleOrDefault(c => c.CompanyCatalogueId == cata.catalogueId);
                    cataDb.IsActive = cata.isActive;
                }
                context.SaveChanges();
            }
        }

        public static void removeCatalogue(List<CatalogueDTO> catalogue)
        {
            using (DeverateContext context = new DeverateContext())
            {
                foreach (var cata in catalogue)
                {
                    CompanyCatalogue cataDb = context.CompanyCatalogue.SingleOrDefault(c => c.CompanyCatalogueId == cata.companyCatalogueId && c.CompanyId == cata.companyId);
                    cataDb.IsActive = cata.isActive;
                }
                context.SaveChanges();
            }
        }
    }
}
