using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestManagementServices.Model
{
    [JsonObject("GeneralStatisticDTO", ItemNullValueHandling = NullValueHandling.Ignore)]
    public class GeneralStatisticDTO
    {
        public List<GeneralStatisticItemDTO> items { get; set; }
        public GeneralStatisticDTO() { }
        public GeneralStatisticDTO(List<GeneralStatisticItemDTO> items)
        {
            this.items = items;
        }
    }
}
