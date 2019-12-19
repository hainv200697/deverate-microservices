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
        public int configId { get; set; }
        public List<CompanyRankDTO> series { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? endDate { get; set; }
        public string name { get; set; }
        public TestedItemDTO tested { get; set; }
        public TotalEmpItemDTO totalAccount { get; set; }



        public RankStatisticItemDTO() { }
        public RankStatisticItemDTO(int configId, List<CompanyRankDTO> series,
            DateTime createDate, DateTime? endDate, string name, TestedItemDTO tested, TotalEmpItemDTO totalAccount)
        {
            this.configId = configId;
            this.series = series;
            this.createDate = createDate;
            this.endDate = endDate;
            this.name = name;
            this.tested = tested;
            this.totalAccount = totalAccount;
        }

    }
    public class TestedItemDTO
    {
        public string name = "Employees do test";
        public int value { get; set; }
        public TestedItemDTO() { }
        public TestedItemDTO(int value)
        {
            this.value = value;
        }
        public TestedItemDTO(int value, string name)
        {
            this.value = value;
            this.name = name;
        }
    }

    public class TotalEmpItemDTO
    {
        public string name = "Total employees";
        public int value { get; set; }
        public TotalEmpItemDTO() { }
        public TotalEmpItemDTO(int value)
        {
            this.value = value;
        }
        public TotalEmpItemDTO(int value, string name)
        {
            this.value = value;
            this.name = name;
        }
    }
}
