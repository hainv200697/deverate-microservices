using AuthenServices.Model;
using AuthenServices.Models;
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


        public static string CheckLogin(DeverateContext context, string username, string password)
        {
            username = username.ToUpper();
            Account account = context.Account.Where(a => a.Username == username && a.IsActive == true).SingleOrDefault();
            if(account == null)
            {
                return null;
            }
            bool? result = false;
            try
            {
                result = BCrypt.Net.BCrypt.Verify(password, account.Password);
            }
            catch (Exception e)
            {
                return null;
            }
            
            if(result.Value == false)
            {
                return null;
            }
            else
            {
                return TokenManager.GenerateToken(account);
            }
        }

        public static string CreateCompanyAccount(DeverateContext context, CompanyManagerDTO cm)
        {
            try
            {
                Account account = new Account();
                var items = cm.fullName.Split(' ');
                string username = items[items.Length - 1];
                for (int i = 0; i < items.Length - 1; i++)
                {
                    username += items[i].ElementAt(0);
                }
                List<Account> accounts = context.Account.ToList();
                username = username.ToUpper() + (accounts[accounts.Count - 1].AccountId + 1);
                username = AppConstrain.RemoveVietnameseTone(username);

                string password = AppConstrain.GeneratePassword();
                string salt = BCrypt.Net.BCrypt.GenerateSalt(13);
                string encodedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

                account.Username = username.ToUpper();
                account.Password = encodedPassword;
                account.Fullname = cm.fullName;
                account.Gender = false;
                account.JoinDate = DateTime.Now;
                account.RoleId = 2;
                account.CompanyId = cm.companyId;
                account.IsActive = true;
                context.Account.Add(account);
                context.SaveChanges();
                return username.ToUpper() + "_" + password;
            }
            catch(Exception e)
            {
                File.WriteAllText(AppConstrain.logFile, e.Message);
            }
            return null;


        }

        public static List<AccountDTO> GetEmployee(int? companyId, bool? status)
        {
            using (DeverateContext context = new DeverateContext())

            {

                var employee = context.Account.Where(acc => acc.CompanyId == companyId &&( acc.RoleId == 3 || acc.RoleId == 4) && acc.IsActive == status).Select(acc => new AccountDTO(acc));
                return employee.ToList();
            }

        }

        public static void UpdateEmployeeStatus(List<int> listEmpId, bool? status)
        {
            using (DeverateContext context = new DeverateContext())

            {
                context.Account.Where(acc => listEmpId.Contains(acc.AccountId)).ToList().ForEach(x => x.IsActive = status);
                context.SaveChanges();
            }

        }

        public static List<string> checkExistedEmail(List<string> listemail,int? companyId)
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

        //public static void updateAccountRole(AccountDTO acc)
        //{
        //    using (DeverateContext context = new DeverateContext())

        //    {
        //        var accountDB = context.Account.SingleOrDefault(x=>x.AccountId == acc.accountId);
        //        accountDB.RoleId = acc.roleId;
        //        context.Account.Update(accountDB);
        //        context.SaveChanges();
        //    }

        //}

        public static List<AccountDTO> GetAccountByRole(int? companyId, bool? status,int? role)
        {
            using (DeverateContext context = new DeverateContext())
            {
                var account = context.Account.Where(acc => acc.CompanyId == companyId &&  acc.RoleId == role && acc.IsActive == status).Select(acc => new AccountDTO(acc));
                return account.ToList();
            }

        }
    }
}
