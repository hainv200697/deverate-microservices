using AuthenServices.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestManagementServices.Models;

namespace AuthenServices.Model
{
    [JsonObject("AccountDTO", ItemNullValueHandling = NullValueHandling.Ignore)]
    public class AccountDTO
    {
        [JsonProperty("AccountId")]
        public int? accountId { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string fullname { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public bool? gender { get; set; }
        public string avatar { get; set; }
        public int? companyId { get; set; }
        public DateTime? joinDate { get; set; }
        public int? roleId { get; set; }
        public bool? isActive { get; set; }

        public AccountDTO()
        {
        }
        public AccountDTO(Account account)
        {
            this.accountId = account.AccountId;
            this.username = account.Username;
            this.password = account.Password;
            this.fullname = account.Fullname;
            this.phone = account.Phone;
            this.email = account.Email;
            this.address = account.Address;
            this.gender = account.Gender;
            this.avatar = account.Avatar;
            this.joinDate = account.JoinDate;
            this.roleId = account.RoleId;
            this.companyId = account.CompanyId;
            this.isActive = account.IsActive;
        }
        public AccountDTO(int? accountId,string username,string fullname,string phone,string email,string address,bool gender)
        {
            this.accountId = accountId;
            this.username = username;
            this.fullname = fullname;
            this.phone = phone;
            this.email = email;
            this.address = address;
            this.gender = gender;
        }
    }
}
