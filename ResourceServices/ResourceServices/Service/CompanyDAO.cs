using AuthenServices.Models;
using Microsoft.EntityFrameworkCore;
using ResourceServices.Model;
using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Service
{
    public class CompanyDAO
    {
        DeverateContext context;

        public CompanyDAO(DeverateContext context)
        {
            this.context = context;
        }


        public static List<CompanyDTO> GetAllCompany(bool isActive)
        {
            using (DeverateContext db = new DeverateContext())
            {
                var company = from com in db.Company
                              join acc in db.Account on com.CompanyId equals acc.CompanyId
                              where com.IsActive == isActive
                              where acc.RoleId == 2
                              select new CompanyDTO(com, acc.Fullname);
                return company.ToList().OrderByDescending(x => x.companyId).ToList();
            }
        }

        public static CompanyDTO GetCompanyById(int? id)
        {
            using (DeverateContext db = new DeverateContext())
            {
                var company = from com in db.Company
                              where com.CompanyId == id
                              select new CompanyDTO(com);
                return company.FirstOrDefault();
            }
        }

        public static List<CompanyDTO> GetCompanyByName(string name)
        {
            using (DeverateContext db = new DeverateContext())
            {
                var company = from com in db.Company
                              where com.Name.Contains(name)
                              select new CompanyDTO(com);
                return company.ToList();
            }
        }

        public static Company CreateCompany(CompanyDataDTO companyData)
        {
            using (DeverateContext db = new DeverateContext())
            {
                Company com = new Company();
                com.Address = companyData.CompanyDTO.address;
                com.Name = companyData.CompanyDTO.name;
                com.CreateAt = DateTime.Now;
                com.Fax = companyData.CompanyDTO.fax;
                com.Phone = companyData.CompanyDTO.phone;
                com.IsActive = companyData.CompanyDTO.isActive;
                var result = db.Company.Add(com);
                var cata = db.Catalogue.Where(x => x.IsActive.Value && !x.Type.Value).ToList();
                // add all catalogue to companyCatalogue
                List<CatalogueInCompany> catalogueInCompany = new List<CatalogueInCompany>();
                foreach(Catalogue catalogue in cata)
                {
                    catalogueInCompany.Add(new CatalogueInCompany
                    {
                        Catalogue = catalogue,
                        IsActive = true,
                    });
                }
                // add to company
                com.CatalogueInCompany = catalogueInCompany;
                db.SaveChanges();
                return result.Entity;
            }
        }

        public static string UpdateCompany(CompanyDTO company)
        {
            using (DeverateContext db = new DeverateContext())
            {
                Company com = db.Company.SingleOrDefault(co => co.CompanyId == company.companyId);
                com.Name = company.name;
                com.Address = company.address;
                com.Fax = company.fax;
                com.Phone = company.phone;
                com.IsActive = company.isActive;
                db.SaveChanges();
                return null;
            }
        }

        public static string DisableCompany(List<CompanyDTO> company)
        {
            using (DeverateContext db = new DeverateContext())
            {
                Company com;
                foreach (var item in company)
                {
                    com = db.Company.SingleOrDefault(co => co.CompanyId == item.companyId);
                    com.Name = item.name;
                    com.Address = item.address;
                    com.IsActive = item.isActive;
                }
                db.SaveChanges();
                return null;
            }
        }
    }
}
