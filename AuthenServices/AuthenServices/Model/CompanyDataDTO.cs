using System;
using AuthenServices.Models;
using AuthenServices.Service;

namespace AuthenServices.Model
{
    public class CompanyDataDTO
    {
        public CompanyDataDTO()
        {

        }

        public CompanyDTO CompanyDTO { get; set; }
        public AccountDTO AccountDTO { get; set; }
    }
}
