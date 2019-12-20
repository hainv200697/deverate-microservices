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
    public class RankDAO
    {
        DeverateContext context;

        public RankDAO(DeverateContext context)
        {
            this.context = context;
        }

        public static List<CompanyRankDTO> getAllCompanyRank(bool isActive, int companyId)
        {
            using (DeverateContext db = new DeverateContext())
            {
                var ranks = db.CompanyRank.Where(c => c.IsActive == isActive && c.CompanyId == companyId).Select(c => new CompanyRankDTO(c));
                return ranks.ToList().OrderByDescending(x => x.companyRankId).ToList();
            }
        }
        public static void createCompanyRank(CompanyRankDTO rankDTO)
        {
            using (DeverateContext db = new DeverateContext())
            {
                CompanyRank companyRank = new CompanyRank();
                companyRank.CompanyId = rankDTO.companyId;
                companyRank.Name = rankDTO.name;
                companyRank.CreateDate = DateTime.UtcNow;
                companyRank.IsActive = true;
                db.CompanyRank.Add(companyRank);
                db.SaveChanges();
            }
        }
        public static void updateCompanyRank(CompanyRankDTO rankDTO)
        {
            using (DeverateContext db = new DeverateContext())
            {
                CompanyRank companyRank = db.CompanyRank.SingleOrDefault(co => co.CompanyRankId == rankDTO.companyRankId);
                companyRank.Name = rankDTO.name;
                db.SaveChanges();
            }
        }
        public static void changeStatusCompanyRank(List<int> rankId, bool status)
        {
            using (DeverateContext db = new DeverateContext())
            {
                db.CompanyRank.Where(x => rankId.Contains(x.CompanyRankId)).ToList().ForEach(x => x.IsActive = status);
                db.SaveChanges();
            }
        }
    }
}
