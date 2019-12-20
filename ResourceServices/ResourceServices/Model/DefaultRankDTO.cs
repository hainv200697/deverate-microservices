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
        public bool isActive { get; set; }

        public DefaultRankDTO()
        {

        }
        public DefaultRankDTO(DefaultRank defaultRank)
        {
            this.defaultRankId = defaultRank.DefaultRankId;
            this.name = defaultRank.Name;
            this.createDate = defaultRank.CreateDate;
            this.isActive = defaultRank.IsActive;
        }
    }
}
