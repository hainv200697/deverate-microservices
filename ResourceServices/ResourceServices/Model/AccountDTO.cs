using AuthenServices.Models;
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
        [JsonProperty("AccountId")]
        public int? AccountId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public bool? Gender { get; set; }
        public string Avatar { get; set; }
        public DateTime? JoinDate { get; set; }
        public int? RoleId { get; set; }
        public bool? IsActive { get; set; }

        public AccountDTO()
        {
        }
        public AccountDTO(Account account)
        {
            this.AccountId = account.AccountId;
            this.Username = account.Username;
            this.Password = account.Password;
            this.Fullname = account.Fullname;
            this.Phone = account.Phone;
            this.Email = account.Email;
            this.Address = account.Address;
            this.Gender = account.Gender;
            this.Avatar = account.Avatar;
            this.JoinDate = account.JoinDate;
            this.RoleId = account.RoleId;
            this.IsActive = account.IsActive;
        }
    }
}
