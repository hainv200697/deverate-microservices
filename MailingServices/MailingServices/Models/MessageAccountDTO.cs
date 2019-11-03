using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailingServices.Models
{
    public class MessageAccountDTO
    {
        public string Username;
        public string Password;
        public string Email;

        public MessageAccountDTO(string username, string password, string email)
        {
            Username = username;
            Password = password;
            Email = email;
        }
    }
}
