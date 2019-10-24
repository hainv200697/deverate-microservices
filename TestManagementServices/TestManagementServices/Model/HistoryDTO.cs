using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestManagementServices.Model
{
    [JsonObject("HistoryDTO", ItemNullValueHandling = NullValueHandling.Ignore)]
    public class HistoryDTO
    {
        public List<TestHistoryDTO> histories { get; set; }
        public HistoryDTO() { }
        public HistoryDTO(List<TestHistoryDTO> histories)
        {
            this.histories = histories;
        }
    }
}
