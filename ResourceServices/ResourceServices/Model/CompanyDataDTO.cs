using AuthenServices.Model;
using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    public class CompanyDataDTO
    {
        public CompanyDataDTO()
        {

        }

        public CompanyDTO CompanyDTO { get; set; }
        public AccountDTO AccountDTO { get; set; }

        public CompanyDataDTO(Company company, Account account)
        {
            this.CompanyDTO = new CompanyDTO(company);
            this.AccountDTO = new AccountDTO(account);
        }
    }
}
