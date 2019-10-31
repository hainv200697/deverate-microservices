using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestManagementServices.Model
{
    [JsonObject("RankStatisticItemDTO", ItemNullValueHandling = NullValueHandling.Ignore)]
    public class RankStatisticItemDTO
    {
        public int? configId { get; set; }
        public List<RankDTO> series { get; set; }
        public DateTime? createDate { get; set; }
        public DateTime? endDate { get; set; }
        public string name { get; set; }


        public RankStatisticItemDTO() { }
        public RankStatisticItemDTO(int? configId, List<RankDTO> series,
            DateTime? createDate, DateTime? endDate, string name)
        {
            this.configId = configId;
            this.series = series;
            this.createDate = createDate;
            this.endDate = endDate;
            this.name = name;
        }

    }
}
