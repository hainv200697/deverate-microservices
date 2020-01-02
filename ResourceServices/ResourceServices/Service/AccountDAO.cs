using AuthenServices.Model;
using AuthenServices.Models;
using Microsoft.EntityFrameworkCore;
using ResourceServices.Model;
using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace AuthenServices.Service
{
    public class AccountDAO
    {
        public static void UpdateEmployeeStatus(List<string> listEmpUsername, bool? status)
        {
            using (DeverateContext context = new DeverateContext())
            {
                context.Account.Where(acc => listEmpUsername.Contains(acc.Username)).ToList().ForEach(x => x.IsActive = status.Value);
                context.SaveChanges();
            }
        }

        public static List<string> checkExistedEmail(List<string> listemail, int? companyId)
        {
            using (DeverateContext context = new DeverateContext())
            {
                var check = context.Account.Where(x => listemail.Contains(x.Email) && x.CompanyId == companyId).Select(x => x.Email).ToList();
                return check;
            }
        }

        public static List<string> checkExistedAccount(List<string> listUsername, int? companyId)
        {
            using (DeverateContext context = new DeverateContext())
            {
                var check = context.Account.Where(x => listUsername.Contains(x.Username) && x.CompanyId == companyId).Select(x => x.Username).ToList();
                return check;
            }
        }

        public static List<AccountDTO> GetAccountByRole(int companyId, int role)
        {
            using (DeverateContext context = new DeverateContext())
            {
                var account = context.Account
                    .Include(x => x.CompanyRank)
                    .Where(acc => acc.CompanyId == companyId &&
                    (role != 0 ? acc.RoleId == role : acc.RoleId != 1))
                    .Select(acc => new AccountDTO(acc, true,acc.CompanyRank));
                return account.ToList();
            }

        }

        public static ProfileDTO GetProfile(int accountId)
        {
            using (DeverateContext db = new DeverateContext())
            {
                return db.Account.Where(x => x.AccountId == accountId).Select(x => new ProfileDTO(x)).FirstOrDefault();
            }
        }

        public static void UpdateProfile(ProfileDTO profileDTO)
        {
            using (DeverateContext db = new DeverateContext())
            {
                var account = db.Account.SingleOrDefault(x => x.AccountId == profileDTO.accountId);
                account.Fullname = profileDTO.fullname;
                account.Phone = profileDTO.phone;
                account.Address = profileDTO.address;
                account.Gender = profileDTO.gender;
                db.SaveChanges();
            }
        }
    }
}
