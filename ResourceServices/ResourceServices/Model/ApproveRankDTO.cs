using ResourceServices.Models;
using System;

namespace AuthenServices.Model
{
    public class ApproveRankDTO
    {
        public int testId { get; set; }
        public int? accountId { get; set; }
        public string username { get; set; }
        public string fullname { get; set; }
        public int? accountRankId { get; set; }
        public string accountRankName { get; set; }
        public int? rankInTestId { get; set; }
        public string rankInTestName { get; set; }

        public ApproveRankDTO()
        {
        }

    }
}
