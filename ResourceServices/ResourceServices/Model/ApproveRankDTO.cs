using ResourceServices.Models;
using System;

namespace AuthenServices.Model
{
    public class ApproveRankDTO
    {
        public int testId { get; set; }
        public int accountId { get; set; }
        public string username { get; set; }
        public string fullname { get; set; }
        private string _accountRankName;
        public string accountRankName
        {
            get { return _accountRankName; }
            set { _accountRankName = value ?? "Updating"; }
        }
        public string rankInTestName { get; set; }

        public ApproveRankDTO()
        {
        }

    }
}
