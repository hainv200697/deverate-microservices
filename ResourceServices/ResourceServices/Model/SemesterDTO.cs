using Newtonsoft.Json;
using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    public class SemesterDTO
    {
        public int accountId { get; set; }
        public string username { get; set; }
        public string fullname { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public bool gender { get; set; }
        public int roleId { get; set; }
        public bool isActive { get; set; }
        public int? companyId { get; set; }
        public string rankName { get; set; }

        public SemesterDTO()
        {

        }

        public SemesterDTO(Account account)
        {
            this.accountId = account.AccountId;
            this.username = account.Username;
            this.fullname = account.Fullname;
            this.phone = account.Phone;
            this.email = account.Email;
            this.address = account.Address;
            this.gender = account.Gender;
            this.roleId = account.RoleId;
            this.isActive = account.IsActive;
            this.companyId = account.CompanyId;
            if (account.CompanyRankId == null)
            {
                this.rankName = "Unknow rank";
            }
            else
            {
                this.rankName = account.CompanyRank.Name;
            }

        }
    }
}
