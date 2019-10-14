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
                var catelogue = from cate in context.Catalogue
                                where cate.IsActive == true
                                select new CatalogueDTO(cate);
                return catelogue.ToList();
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
                    cataDb.IsActive = false;
                    context.SaveChanges();
                }
                return Message.removeCatalogueSucceed;
            }
        }
    }
}
