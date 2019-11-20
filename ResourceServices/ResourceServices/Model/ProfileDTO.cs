using AuthenServices.Models;
using Newtonsoft.Json;
using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    public class ProfileDTO
    {
        public int accountId { get; set; }
        public string username { get; set; }
        public string fullname { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public bool gender { get; set; }
        public string avatar { get; set; }

        public ProfileDTO() { }

        public ProfileDTO(Account account)
        {
            this.accountId = account.AccountId;
            this.username = account.Username;
            this.fullname = account.Fullname;
            this.phone = account.Phone;
            this.email = account.Email;
            this.address = account.Address;
            this.gender = account.Gender.Value;
            this.avatar = account.Avatar;
        }
    }
}
