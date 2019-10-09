using ResourceServices.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenServices.Models;

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
                cata.Description = catalogue.Description;
                cata.Name = catalogue.Name;
                cata.IsActive = catalogue.IsActive;
                context.Catalogue.Add(cata);
                context.SaveChanges();
                return "Creating catalogue success";
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
                return "UPdating catalogue success"; 
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
                    context.SaveChanges();
                }
                return "Removing catalog success";
            }
        }
    }
}
