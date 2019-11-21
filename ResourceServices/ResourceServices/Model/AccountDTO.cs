﻿using AuthenServices.Models;
using Newtonsoft.Json;
using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenServices.Model
{
    [JsonObject("AccountDTO", ItemNullValueHandling = NullValueHandling.Ignore)]
    public class AccountDTO
    {
        [JsonProperty("accountId")]
        public int? accountId { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string fullname { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public bool gender { get; set; }
        public string avatar { get; set; }
        public DateTime? joinDate { get; set; }
        public int roleId { get; set; }
        public bool? isActive { get; set; }
        public int? companyId { get; set; }

        public AccountDTO()
        {
        }
        public AccountDTO(Account account)
        {
            if (account != null)
            {
                this.accountId = account.AccountId;
                this.username = account.Username;
                this.fullname = account.Fullname;
                this.phone = account.Phone;
                this.email = account.Email;
                this.address = account.Address;
                this.gender = account.Gender.Value;
                this.avatar = account.Avatar;
                this.joinDate = account.JoinDate;
                this.roleId = account.RoleId;
                this.isActive = account.IsActive;
            }
            
        }
    }
}
