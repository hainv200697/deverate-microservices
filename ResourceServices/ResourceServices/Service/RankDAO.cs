using AuthenServices.Models;
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

        public static List<RankDTO> GetAllRank(bool isActive)
        {
            using (DeverateContext db = new DeverateContext())
            {
                var ranks = db.CompanyRank.Where(c => c.IsActive == isActive).Select(c => new RankDTO(c));
                return ranks.ToList().OrderByDescending(x => x.rankId).ToList();
            }
        }
    }
}
