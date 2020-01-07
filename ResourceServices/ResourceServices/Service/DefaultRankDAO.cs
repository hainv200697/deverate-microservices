using ResourceServices.Model;
using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Service
{
    public class DefaultRankDAO
    {
        DeverateContext context;

        public DefaultRankDAO(DeverateContext context)
        {
            this.context = context;
        }
        public static List<DefaultRankDTO> getAllDefaultRank(bool isActive)
        {
            using (DeverateContext db = new DeverateContext())
            {
                var ranks = db.Rank.Where(c => c.IsActive == isActive).Select(c => new DefaultRankDTO(c));
                return ranks.ToList();
            }
        }
        public static void createDefaultRank(DefaultRankDTO defaultRankDTO)
        {
            using (DeverateContext db = new DeverateContext())
            {
                Rank defaultRank = new Rank();
                defaultRank.Name = defaultRankDTO.name;
                defaultRank.IsActive = true;
                defaultRank.CreateDate = DateTime.UtcNow;
                db.Rank.Add(defaultRank);
                db.SaveChanges();
            }
        }
        public static void updateDefaultRank(DefaultRankDTO defaultRankDTO)
        {
            using (DeverateContext db = new DeverateContext())
            {
                Rank defaultRank = db.Rank.SingleOrDefault(c => c.RankId == defaultRankDTO.rankId);
                defaultRank.Name = defaultRankDTO.name;
                db.SaveChanges();
            }
        }
        public static void changeStatusDefaultRank(List<int> defaultRankId, bool status)
        {
            using (DeverateContext db = new DeverateContext())
            {
                db.Rank.Where(x => defaultRankId.Contains(x.RankId)).ToList().ForEach(x => x.IsActive = status);
                db.SaveChanges();
            }
        }
        public static bool checkExistedRank(string name)
        {
            using (DeverateContext db = new DeverateContext())
            {
                var check = db.Rank.Where(x => x.Name == name);
                return check.Any();
            }
        }
    }
}
