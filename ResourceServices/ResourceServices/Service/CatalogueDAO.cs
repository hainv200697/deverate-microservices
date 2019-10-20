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
        public static List<CatalogueDTO> GetAllCatalogue(bool status)
        {
            using (DeverateContext context = new DeverateContext())
            {
                var catalogue = from cata in context.Catalogue
                                where cata.IsActive == status
                                select new CatalogueDTO(cata,cata.Question.Count(ques => ques.IsActive == true));
                return catalogue.ToList();
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
                context.Catalogue.Add(cata);
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
                    context.SaveChanges();
                }
                return Message.removeCatalogueSucceed;
            }
        }
    }
}
