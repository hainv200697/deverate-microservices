using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenServices.Model
{
    public class ChangePassRequest
    {
        public string username { get; set; }
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
    }
}
