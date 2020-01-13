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
        public static List<CompanyDTO> GetAllCompany()
        {
            using (DeverateContext db = new DeverateContext())
            {
                var company = db.Company.Include(c => c.Account)
                    .Select(c => new CompanyDTO(c, c.Account.Where(r => r.RoleId == 2).FirstOrDefault()));
                return company.ToList().OrderByDescending(x => x.companyId).ToList();
            }
        }

        public static CompanyDataDTO GetCompanyById(int id)
        {
            using (DeverateContext db = new DeverateContext())
            {
                return db.Company.Include(x => x.Account).Where(x => x.CompanyId == id).Select(x => new CompanyDataDTO(x, x.Account.FirstOrDefault())).FirstOrDefault();
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

        

        public static void UpdateCompany(CompanyDTO company)
        {
            using (DeverateContext db = new DeverateContext())
            {
                Company com = db.Company.SingleOrDefault(co => co.CompanyId == company.companyId);
                com.Name = company.name;
                com.Address = company.address;
                com.Phone = company.phone;
                com.IsActive = company.isActive.Value;
                db.SaveChanges();
            }
        }

        public static void DisableCompany(List<int> company, bool? status)
        {
            using (DeverateContext db = new DeverateContext())
            {
                db.Company.Where(x => company.Contains(x.CompanyId)).ToList().ForEach(x => x.IsActive = status.Value);
                db.SaveChanges();
            }
        }

        public static bool checkExistedCompany(string companyName)
        {
            using (DeverateContext db = new DeverateContext())
            {
                var check = db.Company.Where(x => companyName.Contains(x.Name));
                return check.Any();
            }

        }

    }
}
