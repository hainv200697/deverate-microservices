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
                var ranks = db.Rank.Where(c => c.IsActive == isActive && c.CompanyId == companyId).Select(c => new CompanyRankDTO(c));
                return ranks.ToList().OrderBy(x => x.position).ToList();
            }
        }
        public static List<DefaultRankDTO> getAllDefaultRank()
        {
            using (DeverateContext db = new DeverateContext())
            {
                var ranks = db.Rank.Where(c => c.IsActive == true && c.IsDefault == true).Select(c => new DefaultRankDTO(c));
                return ranks.ToList();
            }
        }

        public static void changeStatusCompanyRank(List<int> rankId, bool status)
        {
            using (DeverateContext db = new DeverateContext())
            {
                db.Rank.Where(x => rankId.Contains(x.RankId)).ToList().ForEach(x => x.IsActive = status);
                db.SaveChanges();
            }
        }
    }
}
