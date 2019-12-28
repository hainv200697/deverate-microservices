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
                var newRanks = rankDTO.Where(x => x.companyRankId == 0);
                var upRanks = rankDTO.Where(x => x.companyRankId != 0);
                foreach (var item in newRanks)
                {
                    CompanyRank companyRank = new CompanyRank
                    {
                        CompanyId = item.companyId,
                        Name = item.name,
                        CreateDate = DateTime.UtcNow,
                        IsActive = true,
                        Position = item.position
                    };
                    db.CompanyRank.Add(companyRank);
                }
                var ids = upRanks.Select(x => x.companyRankId).ToList();
                var updateRanks = db.CompanyRank.Where(x => ids.Contains(x.CompanyRankId));
                foreach (var item in updateRanks)
                {
                    var find = upRanks.FirstOrDefault(x => x.companyRankId == item.CompanyRankId);
                    bool change = false;
                    if (item.Name != find.name)
                    {
                        item.Name = find.name;
                        change = true;
                    }
                    if (item.Position != find.position)
                    {
                        item.Position = find.position;
                        change = true;
                    }
                    if (change) { db.CompanyRank.Update(item); }
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
