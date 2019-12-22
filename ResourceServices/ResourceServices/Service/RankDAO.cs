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
                return ranks.ToList().OrderBy(x => x.position).ToList();
            }
        }
        public static void updateOrCreateRankIfNotExist(List<CompanyRankDTO> rankDTO)
        {
            using (DeverateContext db = new DeverateContext())
            {
                foreach (var item in rankDTO)
                {
                    if (item.companyRankId == 0)
                    {
                        CompanyRank companyRank = new CompanyRank();
                        companyRank.CompanyId = item.companyId;
                        companyRank.Name = item.name;
                        companyRank.CreateDate = DateTime.UtcNow;
                        companyRank.IsActive = true;
                        companyRank.Position = item.position;
                        db.CompanyRank.Add(companyRank);
                    }
                    else
                    {
                        CompanyRank companyRank = db.CompanyRank.SingleOrDefault(co => co.CompanyRankId == item.companyRankId);
                        if (companyRank.Name != item.name)
                        {
                            companyRank.Name = item.name;
                        }
                        if (companyRank.Position != item.position)
                        {
                            companyRank.Position = item.position;
                        }
                        db.CompanyRank.Update(companyRank);
                    }
                }
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
