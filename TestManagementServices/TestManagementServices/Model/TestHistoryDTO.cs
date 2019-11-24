using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestManagementServices.Model
{
    public class TestHistoryDTO
    {
        
        public int? testId { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string title { get; set; }
        [JsonProperty(PropertyName = "series")]
        public List<CatalogueDTO> catalogues { get; set; }
        public double? point { get; set; }
        public int? rankId { get; set; }
        public string rank { get; set; }
        public DateTime? createDate { get; set; }
        public DateTime? startTime { get; set; }
        public TestHistoryDTO() { }
        public TestHistoryDTO(int? testId, string title, List<CatalogueDTO> catalogues, double? point,
                int? rankId, string rank, DateTime? createDate, DateTime? startTime)
        {
            this.testId = testId;
            this.title = title;
            this.catalogues = catalogues;
            this.point = point;
            this.rankId = rankId;
            this.rank = rank;
            this.createDate = createDate;
            this.startTime = startTime;
        }
    }
}
