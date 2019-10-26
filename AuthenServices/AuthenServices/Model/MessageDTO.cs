﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenServices.Models
{
    public class MessageAccountDTO
    {
        public string Username;
        public string Password;
        public string Email;
        public string Fullname;

        public MessageAccountDTO(string username, string password, string email, string fullname)
        {
            Username = username;
            Password = password;
            Email = email;
            Fullname = fullname;
        }
    }
}
