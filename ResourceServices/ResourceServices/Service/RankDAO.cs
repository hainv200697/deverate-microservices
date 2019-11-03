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
                var ranks = from rank in db.Rank
                              where rank.IsActive == isActive
                              select new RankDTO(rank);
                return ranks.ToList().OrderByDescending(x => x.RankId).ToList();
            }
        }
    }
}
