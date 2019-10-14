using ResourceServices.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenServices.Models;
using ResourceServices.Models;

namespace ResourceServices.Service
{
    public class CatalogueDAO
    {
        public static List<CatalogueDTO> GetAllCatalogue()
        {
            using (DeverateContext context = new DeverateContext())
            {
                var catalogue = from cata in context.Catalogue
                                where cata.IsActive == true
                                select new CatalogueDTO(cata,cata.Question.Count(ques => ques.IsActive == true));
                return catalogue.ToList();
            }

        }


        public static CatalogueDTO GetCatalogueById(int id)
        {
            using (DeverateContext context = new DeverateContext())
            {
                var cataDb = context.Catalogue.Select(cata => new CatalogueDTO(cata)).SingleOrDefault(c => c.CatalogueId == id && c.IsActive == true);
                return cataDb;
            }

        }

        public static string CreateCatalogue(CatalogueDTO catalogue)
        {
            using (DeverateContext context = new DeverateContext())
            {
                Catalogue cata = new Catalogue();
                cata.Description = catalogue.Description;
                cata.Name = catalogue.Name;
                cata.IsActive = catalogue.IsActive;
                context.Catalogue.Add(cata);
                context.SaveChanges();
                return Message.createCatalogueSucceed;
            }

        }

        public static string UpdateCatalogue(CatalogueDTO catalogue)
        {
            using (DeverateContext context = new DeverateContext())
            {
                Catalogue cata = context.Catalogue.SingleOrDefault(c => c.CatalogueId == catalogue.CatalogueId);
                cata.Description = catalogue.Description;
                cata.Name = catalogue.Name;
                cata.IsActive = catalogue.IsActive;
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
                    Catalogue cataDb = context.Catalogue.SingleOrDefault(c => c.CatalogueId == cata.CatalogueId);
                    cataDb.IsActive = false;
                    foreach (var item in cataDb.Question.ToList())
                    {
                        item.IsActive = false;
                    }
                    context.SaveChanges();
                }
                return Message.removeCatalogueSucceed;
            }
        }
    }
}
