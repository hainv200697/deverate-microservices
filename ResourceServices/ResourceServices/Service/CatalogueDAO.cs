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
                var catalouge = context.Catalogue.Include(x=>x.Question).Where(x => x.IsActive == status && x.CompanyId == companyId)
                    .Select(x=> new CatalogueDTO(x,x.IsActive,x.Question.Count(ques => ques.IsActive == true)))
                    .ToList();
                return catalouge;
            }
        }

        public static List<CatalogueDefaultDTO> GetAllCatalogueDefault( bool status)
        {
            using (DeverateContext context = new DeverateContext())
            {
                var cata = context.Catalogue
                    .Include(x=> x.Question)
                    .Where(x => x.IsActive == status && x.IsDefault == true)
                    .Select(x=> new CatalogueDefaultDTO(x, x.Question.Count(ques => ques.IsActive == true))).ToList();
                return cata;
            }
        }

        public static void CreateCatalogue(CatalogueDTO catalogue)
        {
            using (DeverateContext context = new DeverateContext())
            {
                var rank = context.Rank.Where(r => r.IsActive == true && r.CompanyId == catalogue.companyId)
                            .Select(r => new CatalogueInRank { 
                                RankId = r.RankId,
                                Point = 0
                            }).ToList();

                Catalogue cata = new Catalogue();
                cata.CompanyId = catalogue.companyId;
                cata.Description = catalogue.description;
                cata.Name = catalogue.name;
                cata.IsActive =true;
                cata.IsDefault = false;
                cata.CreateDate = DateTime.UtcNow;
                if(rank != null)
                {
                    cata.CatalogueInRank = rank;
                }
                context.Catalogue.Add(cata);
                context.SaveChanges();
            }

        }

        public static void CreateCatalogueDefault(CatalogueDefaultDTO catalogue)
        {
            using (DeverateContext context = new DeverateContext())
            {
                var rank = context.Rank.Where(r => r.IsActive == true && r.IsDefault == true)
                            .Select(r => new CatalogueInRank
                            {
                                RankId = r.RankId,
                                Point = 0
                            }).ToList();

                Catalogue cata = new Catalogue();
                cata.Description = catalogue.description;
                cata.Name = catalogue.name;
                cata.IsActive = true;
                cata.IsDefault = true;
                cata.CreateDate = DateTime.UtcNow;
                if (rank != null)
                {
                    cata.CatalogueInRank = rank;
                }
                context.Catalogue.Add(cata);
                context.SaveChanges();
            }

        }

        public static void UpdateCatalogue(CatalogueDTO catalogue)
        {
            using (DeverateContext context = new DeverateContext())
            {
                Catalogue cata = context.Catalogue.SingleOrDefault(c => c.CatalogueId == catalogue.companyCatalogueId);
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
                Catalogue cata = context.Catalogue.SingleOrDefault(c => c.CatalogueId == catalogue.catalogueId);
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
                    Catalogue cataDb = context.Catalogue.SingleOrDefault(c => c.CatalogueId == cata.catalogueId);
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
                    Catalogue cataDb = context.Catalogue.SingleOrDefault(c => c.CatalogueId == cata.companyCatalogueId && c.CompanyId == cata.companyId);
                    cataDb.IsActive = cata.isActive;
                }
                context.SaveChanges();
            }
        }
    }
}
