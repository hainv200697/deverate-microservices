using AuthenServices.Models;
using ResourceServices.Model;
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
                              select new CompanyDTO(acc.Company, acc.Fullname);
                return company.ToList();
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

        public static string CreateCompany(CompanyDTO company)
        {
            using (DeverateContext db = new DeverateContext())
            {
                Company com = new Company();
                com.Address = company.Address;
                com.Name = company.Name;
                com.CreateAt = DateTime.Now;
                com.Fax = company.Fax;
                com.Phone = company.Phone;
                com.IsActive = company.IsActive;
                db.Company.Add(com);
                db.SaveChanges();
                return null;
            }
        }

        public static string UpdateCompany(CompanyDTO company)
        {
            using (DeverateContext db = new DeverateContext())
            {
                Company com = db.Company.SingleOrDefault(co => co.CompanyId == company.CompanyId);
                com.Name = company.Name;
                com.Address = company.Address;
                com.Fax = company.Fax;
                com.Phone = company.Phone;
                com.IsActive = company.IsActive;
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
                    com = db.Company.SingleOrDefault(co => co.CompanyId == item.CompanyId);
                    com.Name = item.Name;
                    com.Address = item.Address;
                    com.IsActive = item.IsActive;
                }
                db.SaveChanges();
                return null;
            }
        }
    }
}
