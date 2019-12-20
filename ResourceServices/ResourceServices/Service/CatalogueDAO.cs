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
                var cata = context.DefaultCatalogue
                    .Include(x=> x.DefaultQuestion)
                    .Where(x => x.IsActive == status)
                    .Select(x=> new CatalogueDefaultDTO(x, x.DefaultQuestion.Count(ques => ques.IsActive == true))).ToList();
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
                cata.CreateDate = DateTime.UtcNow;
                context.CompanyCatalogue.Add(cata);
                context.SaveChanges();
            }

        }

        public static void CreateCatalogueDefault(CatalogueDefaultDTO catalogue)
        {
            using (DeverateContext context = new DeverateContext())
            {
                DefaultCatalogue cata = new DefaultCatalogue
                {
                    Description = catalogue.description,
                    Name = catalogue.name,
                    IsActive = true,
                    CreateDate = DateTime.UtcNow
                };
                context.DefaultCatalogue.Add(cata);
                context.SaveChanges();
            }

        }

        public static void UpdateCatalogue(CatalogueDTO catalogue)
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

        public static void UpdateCatalogueDefault(CatalogueDefaultDTO catalogue)
        {
            using (DeverateContext context = new DeverateContext())
            {
                DefaultCatalogue cata = context.DefaultCatalogue.SingleOrDefault(c => c.DefaultCatalogueId == catalogue.catalogueId);
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
                    DefaultCatalogue cataDb = context.DefaultCatalogue.SingleOrDefault(c => c.DefaultCatalogueId == cata.catalogueId);
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
                    CompanyCatalogue cataDb = context.CompanyCatalogue.SingleOrDefault(c => c.CompanyCatalogueId == cata.catalogueId && c.CompanyId == cata.companyId);
                    cataDb.IsActive = cata.isActive;
                }
                context.SaveChanges();
            }
        }
    }
}
