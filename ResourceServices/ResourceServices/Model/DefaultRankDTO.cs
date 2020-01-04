using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    public class DefaultRankDTO
    {
        public int defaultRankId { get; set; }
        public string name { get; set; }
        public DateTime createDate { get; set; }
        public int position { get; set; }
        public bool isActive { get; set; }

        public DefaultRankDTO()
        {

        }
        public DefaultRankDTO(Rank defaultRank)
        {
            this.defaultRankId = defaultRank.RankId;
            this.name = defaultRank.Name;
            this.createDate = defaultRank.CreateDate;
            this.position = defaultRank.Position;
            this.isActive = defaultRank.IsActive;
        }
    }
}
